using System;
using System.Collections.Generic;
using OnlineOrderingSystem.Models;

namespace OnlineOrderingSystem.DesignPatterns
{
    /// <summary>
    /// Observer pattern implementation for order status notifications
    /// Demonstrates loose coupling between order system and notification system
    /// </summary>
    
    // Observer interface
    public interface IOrderObserver
    {
        void Update(Order order);
    }

    // Subject interface
    public interface IOrderSubject
    {
        void Attach(IOrderObserver observer);
        void Detach(IOrderObserver observer);
        void Notify();
    }

    /// <summary>
    /// Concrete subject - Order class that notifies observers of status changes
    /// </summary>
    public class OrderSubject : IOrderSubject
    {
        private readonly List<IOrderObserver> _observers = new List<IOrderObserver>();
        private readonly Order _order;

        public OrderSubject(Order order)
        {
            _order = order ?? throw new ArgumentNullException(nameof(order));
        }

        public void Attach(IOrderObserver observer)
        {
            if (observer != null && !_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }

        public void Detach(IOrderObserver observer)
        {
            if (observer != null)
            {
                _observers.Remove(observer);
            }
        }

        public void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.Update(_order);
            }
        }

        public void UpdateOrderStatus(string newStatus)
        {
            _order.OrderStatus = newStatus;
            Notify(); // Notify all observers of the status change
        }
    }

    /// <summary>
    /// Concrete observer - Customer notification system
    /// </summary>
    public class CustomerNotificationObserver : IOrderObserver
    {
        private readonly string _customerEmail;

        public CustomerNotificationObserver(string customerEmail)
        {
            _customerEmail = customerEmail ?? throw new ArgumentNullException(nameof(customerEmail));
        }

        public void Update(Order order)
        {
            // Send email notification to customer
            SendEmailNotification(order);
        }

        private void SendEmailNotification(Order order)
        {
            var subject = $"Order #{order.OrderID} Status Update";
            var body = $"Your order status has been updated to: {order.OrderStatus}";
            
            // In a real application, this would use an email service
            Console.WriteLine($"Email sent to {_customerEmail}: {subject} - {body}");
        }
    }

    /// <summary>
    /// Concrete observer - Kitchen notification system
    /// </summary>
    public class KitchenNotificationObserver : IOrderObserver
    {
        public void Update(Order order)
        {
            if (order.OrderStatus == "Preparing")
            {
                NotifyKitchen(order);
            }
        }

        private void NotifyKitchen(Order order)
        {
            // Notify kitchen staff about new order
            Console.WriteLine($"Kitchen notified: Order #{order.OrderID} is ready for preparation");
        }
    }

    /// <summary>
    /// Concrete observer - Delivery notification system
    /// </summary>
    public class DeliveryNotificationObserver : IOrderObserver
    {
        public void Update(Order order)
        {
            if (order.OrderStatus == "Ready for Delivery")
            {
                NotifyDelivery(order);
            }
        }

        private void NotifyDelivery(Order order)
        {
            // Notify delivery staff
            Console.WriteLine($"Delivery notified: Order #{order.OrderID} is ready for delivery");
        }
    }
} 