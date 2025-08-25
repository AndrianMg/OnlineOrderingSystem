using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OnlineOrderingSystem.DesignPatterns;
using OnlineOrderingSystem.Models;
using System.Linq; // Added for FirstOrDefault

namespace OnlineOrderingSystem.Services
{
    /// <summary>
    /// Notification service that implements Observer pattern for order status updates
    /// Provides real-time notifications to customers, kitchen staff, and delivery staff
    /// </summary>
    public class NotificationService
    {
        private static NotificationService? _instance;
        private static readonly object _lock = new object();
        
        // Observer instances for different notification types
        private readonly CustomerNotificationObserver _customerObserver;
        private readonly KitchenNotificationObserver _kitchenObserver;
        private readonly DeliveryNotificationObserver _deliveryObserver;
        
        // List of active orders being monitored
        private readonly List<Order> _monitoredOrders = new List<Order>();

        private NotificationService()
        {
            // Initialize observers
            _customerObserver = new CustomerNotificationObserver();
            _kitchenObserver = new KitchenNotificationObserver();
            _deliveryObserver = new DeliveryNotificationObserver();
        }

        /// <summary>
        /// Gets the singleton instance of NotificationService
        /// </summary>
        public static NotificationService GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new NotificationService();
                    }
                }
            }
            return _instance;
        }

        /// <summary>
        /// Starts monitoring an order for status changes
        /// </summary>
        /// <param name="order">The order to monitor</param>
        /// <param name="customerEmail">Customer email for notifications</param>
        public void StartMonitoringOrder(Order order, string customerEmail)
        {
            if (order == null) return;

            // Set customer email for customer notifications
            _customerObserver.SetCustomerEmail(customerEmail);
            
            // Attach all observers to the order
            order.Attach(_customerObserver);
            order.Attach(_kitchenObserver);
            order.Attach(_deliveryObserver);
            
            // Add to monitored orders list
            if (!_monitoredOrders.Contains(order))
            {
                _monitoredOrders.Add(order);
            }
            
            Console.WriteLine($"Started monitoring Order #{order.OrderID} for customer {customerEmail}");
        }

        /// <summary>
        /// Stops monitoring an order
        /// </summary>
        /// <param name="order">The order to stop monitoring</param>
        public void StopMonitoringOrder(Order order)
        {
            if (order == null) return;

            // Detach all observers
            order.Detach(_customerObserver);
            order.Detach(_kitchenObserver);
            order.Detach(_deliveryObserver);
            
            // Remove from monitored orders
            _monitoredOrders.Remove(order);
            
            Console.WriteLine($"Stopped monitoring Order #{order.OrderID}");
        }

        /// <summary>
        /// Gets all currently monitored orders
        /// </summary>
        public List<Order> GetMonitoredOrders()
        {
            return new List<Order>(_monitoredOrders);
        }

        /// <summary>
        /// Triggers a status update for an order
        /// </summary>
        /// <param name="order">The order to update</param>
        /// <param name="newStatus">The new status</param>
        public void TriggerStatusUpdate(Order order, string newStatus)
        {
            if (order == null) return;

            // Update the order status (this will trigger observer notifications)
            order.UpdateStatus(newStatus);
            
            Console.WriteLine($"Order #{order.OrderID} status updated to: {newStatus}");
        }

        /// <summary>
        /// Sends a custom notification to all observers
        /// </summary>
        /// <param name="message">The notification message</param>
        public void SendCustomNotification(string message)
        {
            // Create a temporary order for broadcasting
            var tempOrder = new Order();
            tempOrder.Attach(_customerObserver);
            tempOrder.Attach(_kitchenObserver);
            tempOrder.Attach(_deliveryObserver);
            
            // Trigger notification
            tempOrder.UpdateStatus("Custom", message);
            
            Console.WriteLine($"Custom notification sent: {message}");
        }

        /// <summary>
        /// Gets notification statistics
        /// </summary>
        public string GetNotificationStats()
        {
            return $"Monitoring {_monitoredOrders.Count} orders\n" +
                   $"Customer notifications: {_customerObserver.NotificationCount}\n" +
                   $"Kitchen notifications: {_kitchenObserver.NotificationCount}\n" +
                   $"Delivery notifications: {_deliveryObserver.NotificationCount}";
        }

        /// <summary>
        /// Updates the status of a specific order and triggers notifications
        /// </summary>
        /// <param name="orderId">The order ID to update</param>
        /// <param name="newStatus">The new status</param>
        public void UpdateOrderStatus(int orderId, string newStatus)
        {
            var order = _monitoredOrders.FirstOrDefault(o => o.OrderID == orderId);
            if (order != null)
            {
                // Update the order status (this will trigger observer notifications)
                order.UpdateStatus(newStatus, $"Status updated to: {newStatus}");
                
                Console.WriteLine($"Order #{orderId} status updated to: {newStatus}");
            }
        }
    }
}

