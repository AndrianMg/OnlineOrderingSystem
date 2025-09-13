using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OnlineOrderingSystem.DesignPatterns;
using OnlineOrderingSystem.Models;
using System.Linq; // Added for FirstOrDefault

namespace OnlineOrderingSystem.Services
{
    /// <summary>
    /// Centralized notification service implementing the Observer Pattern.
    /// 
    /// Demonstrates:
    /// - Observer Pattern for loose coupling
    /// - Singleton Pattern for global access
    /// - Strategy Pattern for different notification types
    /// 
    /// Note: This is demonstration code - not actively used in the current application.
    /// </summary>
    public class NotificationService
    {
        #region Singleton Implementation Fields
        
        /// <summary>
        /// Singleton instance of the NotificationService.
        /// </summary>
        private static NotificationService? _instance;
        
        /// <summary>
        /// Thread synchronization lock for singleton instance creation.
        /// </summary>
        private static readonly object _lock = new object();
        
        #endregion
        
        #region Observer Pattern Fields
        
        /// <summary>
        /// Customer notification observer for order status updates.
        /// </summary>
        private readonly CustomerNotificationObserver _customerObserver;
        
        /// <summary>
        /// Kitchen notification observer for order preparation updates.
        /// </summary>
        private readonly KitchenNotificationObserver _kitchenObserver;
        
        /// <summary>
        /// Delivery notification observer for order delivery updates.
        /// </summary>
        private readonly DeliveryNotificationObserver _deliveryObserver;
        
        #endregion

        #region Events

        /// <summary>
        /// Event triggered when an order's status is updated.
        /// UI components can subscribe to this event to receive real-time updates.
        /// </summary>
        public event Action<Order>? OrderStatusChanged;

        #endregion

        #region Order Monitoring Fields
        
        /// <summary>
        /// Collection of orders currently being monitored for status changes.
        /// </summary>
        private readonly List<Order> _monitoredOrders = new List<Order>();
        
        #endregion

        #region Constructor
        
        /// <summary>
        /// Private constructor implementing the Singleton pattern.
        /// </summary>
        private NotificationService()
        {
            // Initialize all observer instances for different notification types
            // Each observer is responsible for a specific stakeholder group
            _customerObserver = new CustomerNotificationObserver();
            _kitchenObserver = new KitchenNotificationObserver();
            _deliveryObserver = new DeliveryNotificationObserver();
        }
        
        #endregion

        #region Singleton Access Methods
        
        /// <summary>
        /// Gets the singleton instance of the NotificationService.
        /// </summary>
        /// <returns>The singleton instance of NotificationService.</returns>
        public static NotificationService GetInstance()
        {
            // First null check - avoids locking in most cases for better performance
            if (_instance == null)
            {
                // Acquire lock only when instance needs to be created
                lock (_lock)
                {
                    // Second null check - prevents multiple instantiation in race conditions
                    if (_instance == null)
                    {
                        _instance = new NotificationService();
                    }
                }
            }
            return _instance;
        }
        
        #endregion
        
        #region Order Monitoring Methods
        
        /// <summary>
        /// Starts monitoring an order for status changes and enables real-time notifications.
        /// </summary>
        /// <param name="order">The order to monitor for status changes.</param>
        /// <param name="customerEmail">Customer email address for targeted notifications.</param>
        public void StartMonitoringOrder(Order order, string customerEmail)
        {
            // Defensive programming - validate input parameters
            if (order == null) 
                throw new ArgumentNullException(nameof(order), "Order cannot be null");
            
            if (string.IsNullOrWhiteSpace(customerEmail))
                throw new ArgumentException("Customer email cannot be null or empty", nameof(customerEmail));

            // Configure customer observer with specific email for targeted notifications
            // This ensures customer notifications are sent to the correct email address
            _customerObserver.SetCustomerEmail(customerEmail);
            
            // Attach all observers to the order using Observer Pattern
            // Each observer will receive notifications when order status changes
            order.Attach(_customerObserver);  // Customer notifications
            order.Attach(_kitchenObserver);   // Kitchen staff notifications
            order.Attach(_deliveryObserver);  // Delivery staff notifications
            
            // Add order to monitoring collection for efficient lookup and management
            // Duplicate checking prevents multiple monitoring of the same order
            if (!_monitoredOrders.Contains(order))
            {
                _monitoredOrders.Add(order);
            }
            
            // Log monitoring initiation for audit trail and debugging
            Console.WriteLine($"Started monitoring Order #{order.OrderID} for customer {customerEmail}");
        }

        /// <summary>
        /// Stops monitoring an order and cleans up associated resources.
        /// </summary>
        /// <param name="order">The order to stop monitoring.</param>
        public void StopMonitoringOrder(Order order)
        {
            // Defensive programming - handle null orders gracefully
            if (order == null) return;

            // Detach all observers from the order to stop notifications
            // This prevents further notifications from being sent for this order
            order.Detach(_customerObserver);  // Stop customer notifications
            order.Detach(_kitchenObserver);   // Stop kitchen notifications
            order.Detach(_deliveryObserver);  // Stop delivery notifications
            
            // Remove order from monitoring collection for memory management
            // This allows the order to be garbage collected if no other references exist
            _monitoredOrders.Remove(order);
            
            // Log monitoring termination for audit trail and debugging
            Console.WriteLine($"Stopped monitoring Order #{order.OrderID}");
        }

        /// <summary>
        /// Retrieves a snapshot of all currently monitored orders.
        /// </summary>
        /// <returns>A defensive copy of all currently monitored orders.</returns>
        public List<Order> GetMonitoredOrders()
        {
            // Return defensive copy to prevent external modification of internal collection
            // This ensures the integrity of the monitoring system
            return new List<Order>(_monitoredOrders);
        }

        /// <summary>
        /// Triggers a status update for an order and notifies all attached observers.
        /// </summary>
        /// <param name="order">The order to update.</param>
        /// <param name="newStatus">The new status value.</param>
        public void TriggerStatusUpdate(Order order, string newStatus)
        {
            // Defensive programming - validate input parameters
            if (order == null)
                throw new ArgumentNullException(nameof(order), "Order cannot be null");
            
            if (string.IsNullOrWhiteSpace(newStatus))
                throw new ArgumentException("New status cannot be null or empty", nameof(newStatus));

            // Update the order status - this automatically triggers observer notifications
            // The Observer Pattern ensures all attached observers are notified
            order.UpdateStatus(newStatus);
            
            // Log status update for audit trail and debugging
            Console.WriteLine($"Order #{order.OrderID} status updated to: {newStatus}");
        }

        /// <summary>
        /// Sends a custom notification message to all active observers.
        /// </summary>
        /// <param name="message">The custom notification message to broadcast.</param>
        public void SendCustomNotification(string message)
        {
            // Validate input parameter
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Message cannot be null or empty", nameof(message));

            // Create a temporary order object for broadcasting context
            // This provides the necessary structure for observer notifications
            var tempOrder = new Order();
            
            // Attach all observers to the temporary order
            // This ensures all stakeholders receive the custom notification
            tempOrder.Attach(_customerObserver);  // Customer notifications
            tempOrder.Attach(_kitchenObserver);   // Kitchen staff notifications
            tempOrder.Attach(_deliveryObserver);  // Delivery staff notifications
            
            // Trigger notification with custom status and message
            // The "Custom" status identifies this as a custom notification
            tempOrder.UpdateStatus("Custom", message);
            
            // Log custom notification for audit trail and debugging
            Console.WriteLine($"Custom notification sent: {message}");
        }

        /// <summary>
        /// Retrieves comprehensive notification statistics and monitoring metrics.
        /// </summary>
        /// <returns>A formatted string containing comprehensive notification statistics.</returns>
        public string GetNotificationStats()
        {
            // Build comprehensive statistics string with current monitoring and notification data
            // This provides a complete overview of the notification system's state
            return $"Monitoring {_monitoredOrders.Count} orders\n" +
                   $"Customer notifications: {_customerObserver.NotificationCount}\n" +
                   $"Kitchen notifications: {_kitchenObserver.NotificationCount}\n" +
                   $"Delivery notifications: {_deliveryObserver.NotificationCount}";
        }

        /// <summary>
        /// Updates the status of a specific order by ID and triggers notifications.
        /// </summary>
        /// <param name="orderId">The unique identifier of the order to update.</param>
        /// <param name="newStatus">The new status value.</param>
        public void UpdateOrderStatus(int orderId, string newStatus)
        {
            // Validate input parameters
            if (orderId <= 0)
                throw new ArgumentException("Order ID must be positive", nameof(orderId));
            
            if (string.IsNullOrWhiteSpace(newStatus))
                throw new ArgumentException("New status cannot be null or empty", nameof(newStatus));

            // Search for the order in the monitored orders collection
            // Uses LINQ FirstOrDefault for efficient searching by OrderID
            var order = _monitoredOrders.FirstOrDefault(o => o.OrderID == orderId);
            
            // Update order status if found, otherwise handle gracefully
            if (order != null)
            {
                // Update the order status with detailed message
                // This automatically triggers observer notifications
                order.UpdateStatus(newStatus, $"Status updated to: {newStatus}");

                // Trigger the public event for UI updates
                Console.WriteLine($"Triggering OrderStatusChanged event for Order #{orderId}");
                OrderStatusChanged?.Invoke(order);
                Console.WriteLine($"OrderStatusChanged event triggered successfully");

                // Log successful status update for audit trail
                Console.WriteLine($"Order #{orderId} status updated to: {newStatus}");
            }
            // Note: Missing orders are handled silently - no exception thrown
            // This allows for graceful handling of orders that may have been removed
        }
        
        #endregion
    }
}
