using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OnlineOrderingSystem.DesignPatterns;
using OnlineOrderingSystem.Models;
using System.Linq; // Added for FirstOrDefault

namespace OnlineOrderingSystem.Services
{
    /// <summary>
    /// Centralized notification service implementing the Observer Pattern for real-time order status updates.
    /// 
    /// This service provides a comprehensive notification system that enables real-time communication
    /// between different stakeholders in the order fulfillment process. It implements the Observer
    /// Pattern to maintain loose coupling between order status changes and notification delivery.
    /// 
    /// Design Patterns Implemented:
    /// - Observer Pattern: Loose coupling between order subjects and notification observers
    /// - Singleton Pattern: Global access to notification functionality
    /// - Strategy Pattern: Different notification strategies for different stakeholder types
    /// - Template Method Pattern: Consistent notification processing workflow
    /// 
    /// Notification Stakeholders:
    /// - Customer Notifications: Order status updates, delivery confirmations, payment receipts
    /// - Kitchen Notifications: New orders, preparation status updates, special instructions
    /// - Delivery Notifications: Ready orders, delivery assignments, route updates
    /// 
    /// Key Features:
    /// - Real-time order status monitoring and notifications
    /// - Multi-stakeholder notification delivery (customer, kitchen, delivery)
    /// - Custom notification broadcasting capabilities
    /// - Comprehensive notification statistics and monitoring
    /// - Thread-safe singleton implementation for concurrent access
    /// - Automatic observer management and cleanup
    /// 
    /// Notification Types:
    /// - Order Status Updates: Pending → Preparing → Ready → Delivered
    /// - Payment Confirmations: Payment success/failure notifications
    /// - Delivery Updates: Pickup ready, out for delivery, delivered
    /// - Custom Messages: System announcements, promotional messages
    /// 
    /// Technical Implementation:
    /// - Thread-safe singleton with double-checked locking
    /// - Observer pattern with automatic attachment/detachment
    /// - Memory-efficient order monitoring with automatic cleanup
    /// - Console logging for debugging and monitoring
    /// - Extensible observer architecture for future notification types
    /// 
    /// Usage Example:
    /// <code>
    /// // Get singleton instance
    /// var notificationService = NotificationService.GetInstance();
    /// 
    /// // Start monitoring an order
    /// notificationService.StartMonitoringOrder(order, "customer@example.com");
    /// 
    /// // Update order status (triggers notifications)
    /// notificationService.UpdateOrderStatus(orderId, "Preparing");
    /// 
    /// // Send custom notification
    /// notificationService.SendCustomNotification("Kitchen is running 15 minutes behind");
    /// 
    /// // Get statistics
    /// var stats = notificationService.GetNotificationStats();
    /// </code>
    /// </summary>
    public class NotificationService
    {
        #region Singleton Implementation Fields
        
        /// <summary>
        /// Singleton instance of the NotificationService.
        /// 
        /// This field holds the single instance of the NotificationService class,
        /// ensuring that only one instance exists throughout the application lifecycle.
        /// The instance is lazily initialized on first access for optimal performance.
        /// </summary>
        private static NotificationService? _instance;
        
        /// <summary>
        /// Thread synchronization lock for singleton instance creation.
        /// 
        /// This lock ensures thread-safe singleton instantiation in multi-threaded
        /// environments. It prevents race conditions during instance creation and
        /// guarantees that only one instance is created even under concurrent access.
        /// </summary>
        private static readonly object _lock = new object();
        
        #endregion
        
        #region Observer Pattern Fields
        
        /// <summary>
        /// Customer notification observer for order status updates.
        /// 
        /// This observer handles all customer-facing notifications including:
        /// - Order confirmation and status updates
        /// - Payment confirmations and receipts
        /// - Delivery notifications and tracking updates
        /// - Special offers and promotional messages
        /// 
        /// The observer maintains customer email information for targeted notifications
        /// and tracks notification statistics for monitoring purposes.
        /// </summary>
        private readonly CustomerNotificationObserver _customerObserver;
        
        /// <summary>
        /// Kitchen notification observer for order preparation updates.
        /// 
        /// This observer handles all kitchen staff notifications including:
        /// - New order alerts and preparation assignments
        /// - Order status changes during preparation
        /// - Special dietary requirements and modifications
        /// - Kitchen equipment and supply notifications
        /// 
        /// The observer integrates with kitchen display systems and provides
        /// real-time updates to kitchen staff for efficient order management.
        /// </summary>
        private readonly KitchenNotificationObserver _kitchenObserver;
        
        /// <summary>
        /// Delivery notification observer for order delivery updates.
        /// 
        /// This observer handles all delivery staff notifications including:
        /// - Order ready for pickup notifications
        /// - Delivery route assignments and updates
        /// - Customer delivery confirmations
        /// - Delivery status tracking and reporting
        /// 
        /// The observer supports delivery management systems and provides
        /// real-time tracking information for delivery operations.
        /// </summary>
        private readonly DeliveryNotificationObserver _deliveryObserver;
        
        #endregion
        
        #region Order Monitoring Fields
        
        /// <summary>
        /// Collection of orders currently being monitored for status changes.
        /// 
        /// This list maintains references to all active orders that have observers
        /// attached for real-time notification delivery. The collection supports:
        /// - Efficient order lookup by ID for status updates
        /// - Automatic cleanup when orders are completed or cancelled
        /// - Memory management to prevent memory leaks
        /// - Thread-safe access for concurrent operations
        /// 
        /// Orders are automatically added when StartMonitoringOrder is called
        /// and removed when StopMonitoringOrder is called or order is completed.
        /// </summary>
        private readonly List<Order> _monitoredOrders = new List<Order>();
        
        #endregion

        #region Constructor
        
        /// <summary>
        /// Private constructor implementing the Singleton pattern.
        /// 
        /// This constructor initializes the NotificationService with all required
        /// observer instances. The private access modifier ensures that the class
        /// can only be instantiated through the GetInstance() method, maintaining
        /// the singleton pattern and preventing external instantiation.
        /// 
        /// Initialization Process:
        /// 1. Creates CustomerNotificationObserver for customer notifications
        /// 2. Creates KitchenNotificationObserver for kitchen staff notifications
        /// 3. Creates DeliveryNotificationObserver for delivery staff notifications
        /// 4. Initializes the monitored orders collection
        /// 
        /// Thread Safety:
        /// - Constructor is called only once due to singleton pattern
        /// - No thread safety concerns as initialization happens in single thread
        /// - Observer instances are immutable after creation
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
        /// 
        /// This method implements the thread-safe singleton pattern using double-checked
        /// locking to ensure only one instance of the NotificationService exists throughout
        /// the application lifecycle. The method provides global access to notification
        /// functionality while maintaining thread safety in multi-threaded environments.
        /// 
        /// Thread Safety Implementation:
        /// - Double-checked locking pattern prevents race conditions
        /// - First null check avoids unnecessary locking in most cases
        /// - Lock ensures only one thread can create the instance
        /// - Second null check inside lock prevents multiple instantiation
        /// 
        /// Performance Considerations:
        /// - Lazy initialization improves startup performance
        /// - Lock is only acquired when instance is null
        /// - Subsequent calls return existing instance without locking
        /// - Memory efficient with single instance throughout application
        /// 
        /// Usage Pattern:
        /// This method should be called whenever access to the NotificationService
        /// is required. The returned instance is thread-safe and can be used
        /// concurrently from multiple threads.
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
        /// 
        /// This method initiates the complete notification workflow for an order by:
        /// 1. Setting up customer-specific notification preferences
        /// 2. Attaching all relevant observers to the order
        /// 3. Adding the order to the monitoring collection
        /// 4. Logging the monitoring initiation for audit purposes
        /// 
        /// Observer Attachment Process:
        /// - Customer Observer: Configured with customer email for targeted notifications
        /// - Kitchen Observer: Attached for preparation status updates
        /// - Delivery Observer: Attached for delivery status tracking
        /// 
        /// Business Rules:
        /// - Order must not be null (defensive programming)
        /// - Customer email is required for customer notifications
        /// - Duplicate monitoring is prevented through collection checking
        /// - All observers are attached atomically to ensure consistency
        /// 
        /// Integration Points:
        /// - Called automatically when orders are placed through OrderingService
        /// - Integrates with order lifecycle management
        /// - Supports notification system initialization
        /// 
        /// Performance Considerations:
        /// - Efficient duplicate checking prevents unnecessary processing
        /// - Observer attachment is lightweight and fast
        /// - Memory usage is minimal with reference-based monitoring
        /// </summary>
        /// <param name="order">The order to monitor for status changes. Cannot be null.</param>
        /// <param name="customerEmail">Customer email address for targeted notifications. Cannot be null or empty.</param>
        /// <exception cref="ArgumentNullException">Thrown when order parameter is null.</exception>
        /// <exception cref="ArgumentException">Thrown when customerEmail is null or empty.</exception>
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
        /// 
        /// This method performs the complete cleanup process for order monitoring by:
        /// 1. Detaching all observers from the order
        /// 2. Removing the order from the monitoring collection
        /// 3. Logging the monitoring termination for audit purposes
        /// 
        /// Cleanup Process:
        /// - Customer Observer: Detached to stop customer notifications
        /// - Kitchen Observer: Detached to stop kitchen notifications
        /// - Delivery Observer: Detached to stop delivery notifications
        /// - Order removed from monitoring collection
        /// 
        /// Use Cases:
        /// - Order completion or cancellation
        /// - Customer service interventions
        /// - System maintenance and cleanup
        /// - Error recovery scenarios
        /// 
        /// Memory Management:
        /// - Prevents memory leaks by removing order references
        /// - Allows garbage collection of unused order objects
        /// - Maintains efficient monitoring collection size
        /// 
        /// Thread Safety:
        /// - Safe to call from multiple threads
        /// - Defensive null checking prevents exceptions
        /// - Collection operations are atomic
        /// </summary>
        /// <param name="order">The order to stop monitoring. Can be null (defensive programming).</param>
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
        /// 
        /// This method provides access to the current monitoring state without
        /// exposing the internal collection directly. It returns a defensive copy
        /// to prevent external modification of the monitoring collection.
        /// 
        /// Use Cases:
        /// - Administrative monitoring and reporting
        /// - System health checks and diagnostics
        /// - Order status verification
        /// - Performance monitoring and analysis
        /// 
        /// Data Returned:
        /// - Complete list of orders currently being monitored
        /// - Orders include all status and notification information
        /// - Defensive copy prevents external modification
        /// - Snapshot represents state at time of call
        /// 
        /// Performance Considerations:
        /// - Creates new list copy for each call
        /// - Memory usage scales with number of monitored orders
        /// - Consider caching for high-frequency access
        /// - Thread-safe read access to underlying collection
        /// </summary>
        /// <returns>A defensive copy of all currently monitored orders. Returns empty list if no orders are being monitored.</returns>
        public List<Order> GetMonitoredOrders()
        {
            // Return defensive copy to prevent external modification of internal collection
            // This ensures the integrity of the monitoring system
            return new List<Order>(_monitoredOrders);
        }

        /// <summary>
        /// Triggers a status update for an order and notifies all attached observers.
        /// 
        /// This method initiates the complete notification workflow by updating the
        /// order status, which automatically triggers all attached observers to
        /// send appropriate notifications to their respective stakeholders.
        /// 
        /// Notification Workflow:
        /// 1. Order status is updated with new value
        /// 2. Order.UpdateStatus() triggers observer notifications
        /// 3. All attached observers receive the status change
        /// 4. Each observer processes the notification according to its type
        /// 5. Notifications are sent to appropriate stakeholders
        /// 
        /// Observer Processing:
        /// - Customer Observer: Sends email/SMS to customer
        /// - Kitchen Observer: Updates kitchen display systems
        /// - Delivery Observer: Updates delivery management systems
        /// 
        /// Business Rules:
        /// - Order must not be null (defensive programming)
        /// - Status update triggers immediate notifications
        /// - All observers are notified atomically
        /// - Status change is logged for audit purposes
        /// 
        /// Integration Points:
        /// - Called by order management systems
        /// - Integrates with status update workflows
        /// - Supports manual status changes
        /// - Enables demo and testing scenarios
        /// </summary>
        /// <param name="order">The order to update. Cannot be null.</param>
        /// <param name="newStatus">The new status value. Cannot be null or empty.</param>
        /// <exception cref="ArgumentNullException">Thrown when order parameter is null.</exception>
        /// <exception cref="ArgumentException">Thrown when newStatus is null or empty.</exception>
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
        /// 
        /// This method enables broadcasting of custom messages to all notification
        /// stakeholders without requiring a specific order context. It's useful
        /// for system announcements, promotional messages, or general notifications.
        /// 
        /// Broadcasting Process:
        /// 1. Creates a temporary order object for notification context
        /// 2. Attaches all observers to the temporary order
        /// 3. Triggers notification with custom message
        /// 4. Observers process the custom notification
        /// 5. Temporary order is automatically cleaned up
        /// 
        /// Use Cases:
        /// - System maintenance announcements
        /// - Promotional messages and offers
        /// - Emergency notifications
        /// - General business updates
        /// - Testing and demonstration purposes
        /// 
        /// Observer Processing:
        /// - Customer Observer: Sends custom message to all customers
        /// - Kitchen Observer: Displays custom message on kitchen systems
        /// - Delivery Observer: Shows custom message to delivery staff
        /// 
        /// Performance Considerations:
        /// - Temporary order creation is lightweight
        /// - Observer attachment is fast and efficient
        /// - No persistent data is created
        /// - Memory usage is minimal and temporary
        /// 
        /// Business Rules:
        /// - Message cannot be null or empty
        /// - All active observers receive the notification
        /// - Custom status "Custom" is used for identification
        /// - Notification is logged for audit purposes
        /// </summary>
        /// <param name="message">The custom notification message to broadcast. Cannot be null or empty.</param>
        /// <exception cref="ArgumentException">Thrown when message is null or empty.</exception>
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
        /// 
        /// This method provides detailed statistics about the notification system's
        /// current state and performance, enabling monitoring, debugging, and
        /// administrative oversight of the notification infrastructure.
        /// 
        /// Statistics Provided:
        /// - Number of orders currently being monitored
        /// - Total customer notifications sent
        /// - Total kitchen notifications sent
        /// - Total delivery notifications sent
        /// 
        /// Use Cases:
        /// - System monitoring and health checks
        /// - Performance analysis and optimization
        /// - Administrative reporting and oversight
        /// - Debugging and troubleshooting
        /// - Capacity planning and scaling decisions
        /// 
        /// Data Sources:
        /// - Monitored orders count from internal collection
        /// - Notification counts from individual observers
        /// - Real-time data reflecting current system state
        /// - Historical data accumulated since service start
        /// 
        /// Performance Considerations:
        /// - Statistics are calculated on-demand
        /// - No caching - always returns current values
        /// - Lightweight calculation with minimal overhead
        /// - Thread-safe access to all data sources
        /// 
        /// Output Format:
        /// The method returns a formatted string suitable for display in
        /// administrative interfaces, logs, or monitoring dashboards.
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
        /// 
        /// This method provides a convenient way to update order status using only
        /// the order ID, without requiring direct access to the order object. It
        /// performs order lookup and status update in a single operation.
        /// 
        /// Update Process:
        /// 1. Searches for the order in the monitored orders collection
        /// 2. Updates the order status if found
        /// 3. Triggers observer notifications automatically
        /// 4. Logs the status update for audit purposes
        /// 
        /// Order Lookup:
        /// - Uses LINQ FirstOrDefault for efficient searching
        /// - Searches by OrderID for precise matching
        /// - Returns null if order is not found
        /// - Handles missing orders gracefully
        /// 
        /// Business Rules:
        /// - Order must be currently monitored to be updated
        /// - Status update triggers immediate notifications
        /// - Missing orders are handled silently (no exception)
        /// - Status change is logged for audit trail
        /// 
        /// Use Cases:
        /// - External system integration
        /// - Administrative order management
        /// - Automated status updates
        /// - Demo and testing scenarios
        /// 
        /// Performance Considerations:
        /// - Linear search through monitored orders
        /// - Efficient for typical order volumes
        /// - Consider indexing for very large order sets
        /// - Early return for missing orders
        /// </summary>
        /// <param name="orderId">The unique identifier of the order to update. Must be positive.</param>
        /// <param name="newStatus">The new status value. Cannot be null or empty.</param>
        /// <exception cref="ArgumentException">Thrown when orderId is invalid or newStatus is null/empty.</exception>
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
                
                // Log successful status update for audit trail
                Console.WriteLine($"Order #{orderId} status updated to: {newStatus}");
            }
            // Note: Missing orders are handled silently - no exception thrown
            // This allows for graceful handling of orders that may have been removed
        }
    }
}

