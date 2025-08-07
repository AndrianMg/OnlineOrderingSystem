using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OnlineOrderingSystem.Data;
using OnlineOrderingSystem.Models;

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
                context.Orders.Add(order);
                context.SaveChanges();
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
                    .Where(o => o.OrderStatus.Equals(status, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(o => o.OrderDate)
                    .ToList();
            }
        }

        /// <summary>
        /// Updates an existing order
        /// </summary>
        /// <param name="order">The order to update</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool UpdateOrder(Order order)
        {
            using (var context = new OrderingDbContext())
            {
                context.Orders.Update(order);
                return context.SaveChanges() > 0;
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
                var order = context.Orders.Find(orderId);
                if (order != null)
                {
                    order.UpdateStatus(newStatus, message);
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
        /// Gets orders within a date range
        /// </summary>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <returns>List of orders within the date range</returns>
        public List<Order> GetOrdersByDateRange(DateTime startDate, DateTime endDate)
        {
            using (var context = new OrderingDbContext())
            {
                return context.Orders
                    .Include(o => o.OrderItems)
                    .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                    .OrderByDescending(o => o.OrderDate)
                    .ToList();
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
        /// Gets completed orders
        /// </summary>
        /// <returns>List of completed orders</returns>
        public List<Order> GetCompletedOrders()
        {
            return GetOrdersByStatus("Completed");
        }

        /// <summary>
        /// Deletes an order by ID
        /// </summary>
        /// <param name="orderId">The order ID to delete</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool DeleteOrder(int orderId)
        {
            using (var context = new OrderingDbContext())
            {
                var order = context.Orders.Find(orderId);
                if (order != null)
                {
                    context.Orders.Remove(order);
                    return context.SaveChanges() > 0;
                }
                return false;
            }
        }

        /// <summary>
        /// Gets total sales revenue within a date range
        /// </summary>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <returns>Total sales revenue</returns>
        public decimal GetTotalSales(DateTime startDate, DateTime endDate)
        {
            using (var context = new OrderingDbContext())
            {
                return (decimal)context.Orders
                    .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate && 
                               o.OrderStatus == "Completed")
                    .Sum(o => o.TotalAmount);
            }
        }
    }
} 