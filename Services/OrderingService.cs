using System;
using System.Collections.Generic;
using System.Linq;
using OnlineOrderingSystem.Models;
using OnlineOrderingSystem.Data;

namespace OnlineOrderingSystem.Services
{
    /// <summary>
    /// Concrete implementation of the ordering service
    /// Demonstrates dependency injection, strategy pattern, and exception handling
    /// </summary>
    public class OrderingService : IOrderingService
    {
        private readonly OrderingDbContext _context;
        private readonly GlobalClass _globalClass;

        /// <summary>
        /// Constructor with dependency injection
        /// </summary>
        /// <param name="context">The data context</param>
        public OrderingService(OrderingDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _globalClass = GlobalClass.GetInstance();
        }

        /// <summary>
        /// Gets all available items
        /// </summary>
        /// <returns>List of all items</returns>
        public List<Item> GetAllItems()
        {
            try
            {
                _globalClass.LogEvent("Service", "Retrieved all items");
                return _context.GetAllItems().Where(item => item.Availability).ToList();
            }
            catch (Exception ex)
            {
                _globalClass.LogEvent("Error", $"Failed to retrieve items: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Gets items by category
        /// </summary>
        /// <param name="category">The category to filter by</param>
        /// <returns>List of items in the category</returns>
        public List<Item> GetItemsByCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                throw new ArgumentException("Category cannot be null or empty", nameof(category));
            }

            try
            {
                _globalClass.LogEvent("Service", $"Retrieved items for category: {category}");
                return _context.GetItemsByCategory(category).Where(item => item.Availability).ToList();
            }
            catch (Exception ex)
            {
                _globalClass.LogEvent("Error", $"Failed to retrieve items for category {category}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Places an order
        /// </summary>
        /// <param name="customerID">The customer ID</param>
        /// <param name="itemIDs">List of item IDs to order</param>
        /// <returns>The created order</returns>
        public Order PlaceOrder(int customerID, List<int> itemIDs)
        {
            if (itemIDs == null || itemIDs.Count == 0)
            {
                throw new ArgumentException("Item IDs list cannot be null or empty", nameof(itemIDs));
            }

            try
            {
                var customer = GetCustomerById(customerID);
                if (customer == null)
                {
                    throw new ArgumentException($"Customer with ID {customerID} not found");
                }

                var items = _context.GetAllItems().Where(item => itemIDs.Contains(item.ItemID)).ToList();
                if (items.Count != itemIDs.Count)
                {
                    throw new ArgumentException("Some items were not found");
                }

                // Create cart with items
                var cart = new Cart { CustomerID = customerID };
                foreach (var item in items)
                {
                    cart.AddItem(item, 1);
                }

                // Create order
                var order = new Order();
                order.CreateOrder(customerID, cart);

                // Add order to context
                _context.AddOrder(order);

                _globalClass.LogEvent("Order", $"Order {order.OrderID} placed by customer {customerID}");

                return order;
            }
            catch (Exception ex)
            {
                _globalClass.LogEvent("Error", $"Failed to place order for customer {customerID}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Gets customer by ID
        /// </summary>
        /// <param name="customerID">The customer ID</param>
        /// <returns>The customer or null if not found</returns>
        public Customer? GetCustomerById(int customerID)
        {
            try
            {
                return _context.GetCustomerById(customerID);
            }
            catch (Exception ex)
            {
                _globalClass.LogEvent("Error", $"Failed to retrieve customer {customerID}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Processes a payment using strategy pattern
        /// </summary>
        /// <param name="payment">The payment to process</param>
        /// <returns>True if payment was successful, false otherwise</returns>
        public bool ProcessPayment(Payment payment)
        {
            if (payment == null)
            {
                throw new ArgumentNullException(nameof(payment));
            }

            try
            {
                // Use strategy pattern - different payment types have different processing logic
                payment.ProcessPayment();
                
                var success = payment.GetPaymentStatus() == "Completed";
                _globalClass.LogEvent("Payment", $"Payment processed: {success}");

                if (success)
                {
                    _globalClass.SendNotification($"Payment of ${payment.GetAmount():F2} processed successfully", "admin@example.com");
                }

                return success;
            }
            catch (Exception ex)
            {
                _globalClass.LogEvent("Error", $"Payment processing failed: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Gets order history for a customer
        /// </summary>
        /// <param name="customerID">The customer ID</param>
        /// <returns>List of orders for the customer</returns>
        public List<Order> GetOrderHistory(int customerID)
        {
            try
            {
                var orders = _context.GetOrdersByCustomer(customerID);
                _globalClass.LogEvent("Service", $"Retrieved order history for customer {customerID}");
                return orders;
            }
            catch (Exception ex)
            {
                _globalClass.LogEvent("Error", $"Failed to retrieve order history for customer {customerID}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Gets available payment methods
        /// </summary>
        /// <returns>List of available payment methods</returns>
        public List<string> GetAvailablePaymentMethods()
        {
            return new List<string> { "Cash", "Credit", "Check" };
        }

        /// <summary>
        /// Creates a payment object based on payment method
        /// </summary>
        /// <param name="paymentMethod">The payment method</param>
        /// <param name="amount">The payment amount</param>
        /// <returns>The payment object</returns>
        public Payment CreatePayment(string paymentMethod, double amount)
        {
            switch (paymentMethod.ToLower())
            {
                case "cash":
                    return new Cash { AmountTendered = amount, TotalAmount = amount };
                case "credit":
                    var credit = new Credit { CardNumber = "1234567890123456", CardHolderName = "Test User", ExpiryDate = DateTime.Now.AddYears(1), CVV = 123 };
                    credit.SetAmount(amount);
                    return credit;
                case "check":
                    var check = new Check { ChequeNumber = "123456", BankName = "Test Bank" };
                    check.SetAmount(amount);
                    return check;
                default:
                    throw new ArgumentException($"Unsupported payment method: {paymentMethod}");
            }
        }
    }
} 