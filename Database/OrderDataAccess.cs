using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OnlineOrderingSystem.Data;
using OnlineOrderingSystem.Models;
using OnlineOrderingSystem.Services;

namespace OnlineOrderingSystem.Database
{
    /// <summary>
    /// Data access layer for orders using Entity Framework Core
    /// </summary>
    public class OrderDataAccess
    {
        /// <summary>
        /// Creates a new order in the database
        /// </summary>
        /// <param name="order">The order to create</param>
        /// <returns>The created order with its generated ID</returns>
        public Order CreateOrder(Order order)
        {
            using (var context = new OrderingDbContext())
            {
                // Ensure OrderItems and StatusHistory are properly configured
                foreach (var item in order.OrderItems)
                {
                    item.OrderID = 0; // Reset to allow EF to generate new IDs
                }

                foreach (var status in order.StatusHistory)
                {
                    status.OrderID = 0; // Reset to allow EF to generate new IDs
                }

                context.Orders.Add(order);
                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex)

                {
                    Console.WriteLine($"Error saving order: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                    }
                    throw;
                }

                // Update the order with the generated ID
                return order;
            }
        }

        /// <summary>
        /// Creates a new order with payment integration
        /// </summary>
        /// <param name="order">The order to create</param>
        /// <param name="payment">The payment for the order</param>
        /// <param name="customerEmail">Customer email for notifications</param>
        /// <returns>The created order with payment information</returns>
        public Order CreateOrderWithPayment(Order order, Payment payment, string customerEmail = "customer@example.com")
        {
            using (var context = new OrderingDbContext())
            {
                // Create the order first
                context.Orders.Add(order);
                context.SaveChanges();

                // For now, skip payment entity creation since PaymentEntities table doesn't exist
                // Just update the order payment status
                order.PaymentStatus = "Pending";
                context.Orders.Update(order);
                context.SaveChanges();

                // Integrate with notification system
                var notificationService = NotificationService.GetInstance();
                notificationService.StartMonitoringOrder(order, customerEmail);

                return order;
            }
        }

        /// <summary>
        /// Retrieves an order by ID with its order details
        /// </summary>
        /// <param name="orderId">The order ID</param>
        /// <returns>The order or null if not found</returns>
        public Order? GetOrderById(int orderId)
        {
            using (var context = new OrderingDbContext())
            {
                return context.Orders
                    .Include(o => o.OrderItems)
                    .Include(o => o.StatusHistory)
                    .FirstOrDefault(o => o.OrderID == orderId);
            }
        }

        /// <summary>
        /// Retrieves all orders for a specific customer
        /// </summary>
        /// <param name="customerId">The customer ID</param>
        /// <returns>List of orders for the customer</returns>
        public List<Order> GetOrdersByCustomerId(int customerId)
        {
            using (var context = new OrderingDbContext())
            {
                return context.Orders
                    .Include(o => o.OrderItems)
                    .Include(o => o.StatusHistory)
                    .Where(o => o.CustomerID == customerId)
                    .OrderByDescending(o => o.OrderDate)
                    .ToList();
            }
        }

        /// <summary>
        /// Retrieves all orders with a specific status
        /// </summary>
        /// <param name="status">The order status to filter by</param>
        /// <returns>List of orders with the specified status</returns>
        public List<Order> GetOrdersByStatus(string status)
        {
            using (var context = new OrderingDbContext())
            {
                return context.Orders
                    .Include(o => o.OrderItems)
                    .Include(o => o.StatusHistory)
                    .Where(o => o.OrderStatus.ToLower() == status.ToLower())
                    .OrderBy(o => o.OrderDate)
                    .ToList();
            }
        }

        /// <summary>
        /// Updates the status of an order
        /// </summary>
        /// <param name="orderId">The order ID</param>
        /// <param name="newStatus">The new status</param>
        /// <param name="message">Optional status message</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool UpdateOrderStatus(int orderId, string newStatus, string message = "")
        {
            using (var context = new OrderingDbContext())
            {
                var order = context.Orders
                    .Include(o => o.StatusHistory)
                    .FirstOrDefault(o => o.OrderID == orderId);
                
                if (order != null)
                {
                    order.UpdateStatus(newStatus, message);
                    
                    // Save the new status update
                    var latestStatus = order.StatusHistory.Last();
                    context.OrderStatusUpdates.Add(latestStatus);
                    
                    context.Orders.Update(order);
                    return context.SaveChanges() > 0;
                }
                return false;
            }
        }

        /// <summary>
        /// Cancels an order
        /// </summary>
        /// <param name="orderId">The order ID to cancel</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool CancelOrder(int orderId)
        {
            using (var context = new OrderingDbContext())
            {
                var order = context.Orders.Find(orderId);
                if (order != null)
                {
                    order.CancelOrder();
                    context.Orders.Update(order);
                    return context.SaveChanges() > 0;
                }
                return false;
            }
        }

        /// <summary>
        /// Gets pending orders
        /// </summary>
        /// <returns>List of pending orders</returns>
        public List<Order> GetPendingOrders()
        {
            return GetOrdersByStatus("Pending");
        }

        /// <summary>
        /// Gets complete order history for a customer with payment information
        /// </summary>
        /// <param name="customerId">The customer ID</param>
        /// <returns>List of orders with payment details</returns>
        public List<OrderWithPayment> GetOrderHistoryWithPayments(int customerId)
        {
            using (var context = new OrderingDbContext())
            {
                // Since PaymentEntities table doesn't exist, just return orders without payment details
                var orders = context.Orders
                    .Include(o => o.OrderItems)
                    .Include(o => o.StatusHistory)
                    .Where(o => o.CustomerID == customerId)
                    .OrderByDescending(o => o.OrderDate)
                    .ToList();

                // Convert to OrderWithPayment objects with null payment info
                return orders.Select(order => new OrderWithPayment
                {
                    Order = order,
                    Payment = null // Payment details not available since PaymentEntities table doesn't exist
                }).ToList();
            }
        }

        /// <summary>
        /// Gets order statistics for a customer
        /// </summary>
        /// <param name="customerId">The customer ID</param>
        /// <returns>Tuple containing total orders, total spent, and average order value</returns>
        public (int totalOrders, double totalSpent, double averageOrderValue) GetOrderStatistics(int customerId)
        {
            using (var context = new OrderingDbContext())
            {
                var orders = context.Orders
                    .Where(o => o.CustomerID == customerId && o.OrderStatus == "Completed")
                    .ToList();

                var totalOrders = orders.Count;
                var totalSpent = orders.Sum(o => o.TotalAmount);
                var averageOrderValue = totalOrders > 0 ? totalSpent / totalOrders : 0;

                return (totalOrders, totalSpent, averageOrderValue);
            }
        }
    }

    /// <summary>
    /// DTO for order with payment information
    /// </summary>
    public class OrderWithPayment
    {
        public Order Order { get; set; } = new();
        public PaymentEntity? Payment { get; set; }
    }
}