using System;
using System.Collections.Generic;
using System.Linq; // Added for .Sum() and .FirstOrDefault()

namespace OnlineOrderingSystem.Models
{
    /// <summary>
    /// Represents an order in the Tasty Eats online ordering system
    /// </summary>
    public class Order
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public int RestaurantID { get; set; }
        public List<OrderDetail> OrderItems { get; set; } = new List<OrderDetail>();
        public double TotalAmount { get; set; }
        public string OrderStatus { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;
        public string DeliveryAddress { get; set; } = string.Empty;
        
        // New properties for Tasty Eats
        public DateTime EstimatedDeliveryTime { get; set; }
        public DateTime? ActualDeliveryTime { get; set; }
        public string DeliveryInstructions { get; set; } = string.Empty;
        public string SpecialRequests { get; set; } = string.Empty;
        public double DeliveryFee { get; set; }
        public double TaxAmount { get; set; }
        public double Subtotal { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public bool IsDelivery { get; set; } = true;
        public string OrderNotes { get; set; } = string.Empty;
        public List<OrderStatusUpdate> StatusHistory { get; set; } = new List<OrderStatusUpdate>();

        /// <summary>
        /// Creates a new order from a customer's cart
        /// </summary>
        /// <param name="customerID">The customer ID</param>
        /// <param name="cart">The cart containing items</param>
        public void CreateOrder(int customerID, Cart cart)
        {
            CustomerID = customerID;
            OrderDate = DateTime.Now;
            OrderStatus = "Pending";
            PaymentStatus = "Pending";
            EstimatedDeliveryTime = DateTime.Now.AddMinutes(45); // Default 45 minutes

            foreach (var cartItem in cart.Items)
            {
                var orderDetail = new OrderDetail
                {
                    ItemID = cartItem.Item.ItemID,
                    ItemName = cartItem.Item.Name,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Item.Price,
                    TotalPrice = cartItem.TotalPrice
                };
                OrderItems.Add(orderDetail);
            }

            CalculateTotal();
            AddStatusUpdate("Order Created", "Your order has been placed successfully");
        }

        /// <summary>
        /// Calculates the total amount for the order
        /// </summary>
        /// <returns>The total amount</returns>
        public double CalculateTotal()
        {
            Subtotal = OrderItems.Sum(item => item.TotalPrice);
            TaxAmount = Subtotal * 0.20; // 20% VAT for UK
            TotalAmount = Subtotal + TaxAmount + DeliveryFee;
            return TotalAmount;
        }

        /// <summary>
        /// Adds an item to the order
        /// </summary>
        /// <param name="item">The item to add</param>
        /// <param name="quantity">The quantity</param>
        /// <param name="customizations">Optional customizations</param>
        public void AddItem(Item item, int quantity, List<string>? customizations = null)
        {
            var orderDetail = new OrderDetail
            {
                ItemID = item.ItemID,
                ItemName = item.Name,
                Quantity = quantity,
                Price = item.Price,
                TotalPrice = item.Price * quantity,
                Customizations = customizations ?? new List<string>()
            };
            OrderItems.Add(orderDetail);
            CalculateTotal();
        }

        /// <summary>
        /// Removes an item from the order
        /// </summary>
        /// <param name="item">The item to remove</param>
        public void RemoveItem(Item item)
        {
            var orderDetail = OrderItems.FirstOrDefault(od => od.ItemID == item.ItemID);
            if (orderDetail != null)
            {
                OrderItems.Remove(orderDetail);
                CalculateTotal();
            }
        }

        /// <summary>
        /// Gets the order details
        /// </summary>
        /// <returns>List of order details</returns>
        public List<OrderDetail> GetOrderDetails()
        {
            return OrderItems;
        }

        /// <summary>
        /// Cancels the order
        /// </summary>
        public void CancelOrder()
        {
            OrderStatus = "Cancelled";
            PaymentStatus = "Refunded";
            AddStatusUpdate("Order Cancelled", "Your order has been cancelled");
        }

        /// <summary>
        /// Updates the order status
        /// </summary>
        /// <param name="newStatus">The new status</param>
        /// <param name="message">Status message</param>
        public void UpdateStatus(string newStatus, string message = "")
        {
            OrderStatus = newStatus;
            AddStatusUpdate(newStatus, message);
        }

        /// <summary>
        /// Adds a status update to the order history
        /// </summary>
        /// <param name="status">The status</param>
        /// <param name="message">The message</param>
        private void AddStatusUpdate(string status, string message)
        {
            var statusUpdate = new OrderStatusUpdate
            {
                Status = status,
                Message = message,
                Timestamp = DateTime.Now
            };
            StatusHistory.Add(statusUpdate);
        }

        /// <summary>
        /// Tracks the order status
        /// </summary>
        /// <returns>Current order status</returns>
        public string TrackOrder()
        {
            return $"Order {OrderID}: {OrderStatus} - Total: £{TotalAmount:F2}";
        }

        /// <summary>
        /// Gets the order summary
        /// </summary>
        /// <returns>Formatted order summary</returns>
        public string GetOrderSummary()
        {
            return $"Order #{OrderID} - {OrderItems.Count} items - £{TotalAmount:F2} - {OrderStatus}";
        }

        /// <summary>
        /// Checks if order is ready for delivery
        /// </summary>
        /// <returns>True if ready, false otherwise</returns>
        public bool IsReadyForDelivery()
        {
            return OrderStatus == "Ready" && PaymentStatus == "Completed";
        }

        /// <summary>
        /// Marks order as delivered
        /// </summary>
        public void MarkAsDelivered()
        {
            ActualDeliveryTime = DateTime.Now;
            UpdateStatus("Delivered", "Your order has been delivered");
        }

        /// <summary>
        /// Gets the latest status update
        /// </summary>
        /// <returns>The latest status update</returns>
        public OrderStatusUpdate? GetLatestStatus()
        {
            return StatusHistory.OrderByDescending(s => s.Timestamp).FirstOrDefault();
        }
    }

    /// <summary>
    /// Represents a status update for an order
    /// </summary>
    public class OrderStatusUpdate
    {
        public int Id { get; set; }
        public int OrderID { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
} 