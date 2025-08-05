using System;
using System.Collections.Generic;
using OnlineOrderingSystem.Models;
using System.Linq; // Added for .Where() and .ToList()

namespace OnlineOrderingSystem.Data
{
    /// <summary>
    /// Simple in-memory data context for demonstration purposes
    /// In a real application, this would be replaced with Entity Framework DbContext
    /// </summary>
    public class OrderingDbContext
    {
        private static OrderingDbContext? _instance;
        private static readonly object _lock = new object();

        public List<Customer> Customers { get; set; } = new List<Customer>();
        public List<Order> Orders { get; set; } = new List<Order>();
        public List<Item> Items { get; set; } = new List<Item>();
        public List<Cart> Carts { get; set; } = new List<Cart>();
        public List<Payment> Payments { get; set; } = new List<Payment>();

        private OrderingDbContext()
        {
            InitializeSampleData();
        }

        /// <summary>
        /// Gets the singleton instance of the data context
        /// </summary>
        /// <returns>The singleton instance</returns>
        public static OrderingDbContext GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new OrderingDbContext();
                    }
                }
            }
            return _instance;
        }

        /// <summary>
        /// Initializes sample data for demonstration
        /// </summary>
        private void InitializeSampleData()
        {
            // Initialize sample items
            Items.AddRange(new[]
            {
                new Item { ItemID = 1, Name = "Margherita Pizza", Description = "Classic tomato and mozzarella", Price = 12.99, Category = "Pizza", Availability = true },
                new Item { ItemID = 2, Name = "Pepperoni Pizza", Description = "Spicy pepperoni with cheese", Price = 14.99, Category = "Pizza", Availability = true },
                new Item { ItemID = 3, Name = "Caesar Salad", Description = "Fresh romaine with caesar dressing", Price = 8.99, Category = "Salad", Availability = true },
                new Item { ItemID = 4, Name = "Chicken Wings", Description = "Crispy wings with choice of sauce", Price = 10.99, Category = "Appetizer", Availability = true },
                new Item { ItemID = 5, Name = "Chocolate Cake", Description = "Rich chocolate layer cake", Price = 6.99, Category = "Dessert", Availability = true }
            });

            // Initialize sample customers
            Customers.AddRange(new[]
            {
                new Customer { CustomerID = 1, Name = "John Doe", Email = "john@example.com", Address = "123 Main St", PreferredPaymentMethod = "Credit" },
                new Customer { CustomerID = 2, Name = "Jane Smith", Email = "jane@example.com", Address = "456 Oak Ave", PreferredPaymentMethod = "Cash" }
            });

            // Initialize sample carts
            Carts.AddRange(new[]
            {
                new Cart { CartID = 1, CustomerID = 1 },
                new Cart { CartID = 2, CustomerID = 2 }
            });
        }

        /// <summary>
        /// Saves changes to the data context
        /// </summary>
        public void SaveChanges()
        {
            // In a real application, this would persist changes to the database
            Console.WriteLine("Changes saved to database.");
        }

        /// <summary>
        /// Gets all items
        /// </summary>
        /// <returns>List of all items</returns>
        public List<Item> GetAllItems()
        {
            return Items;
        }

        /// <summary>
        /// Gets items by category
        /// </summary>
        /// <param name="category">The category to filter by</param>
        /// <returns>List of items in the specified category</returns>
        public List<Item> GetItemsByCategory(string category)
        {
            return Items.Where(item => item.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        /// <summary>
        /// Gets a customer by ID
        /// </summary>
        /// <param name="customerID">The customer ID</param>
        /// <returns>The customer or null if not found</returns>
        public Customer? GetCustomerById(int customerID)
        {
            return Customers.FirstOrDefault(c => c.CustomerID == customerID);
        }

        /// <summary>
        /// Adds a new order
        /// </summary>
        /// <param name="order">The order to add</param>
        public void AddOrder(Order order)
        {
            order.OrderID = Orders.Count + 1;
            Orders.Add(order);
            SaveChanges();
        }

        /// <summary>
        /// Gets orders for a customer
        /// </summary>
        /// <param name="customerID">The customer ID</param>
        /// <returns>List of orders for the customer</returns>
        public List<Order> GetOrdersByCustomer(int customerID)
        {
            return Orders.Where(o => o.CustomerID == customerID).ToList();
        }
    }
} 