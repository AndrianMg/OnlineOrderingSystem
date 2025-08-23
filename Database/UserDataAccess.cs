using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OnlineOrderingSystem.Data;
using OnlineOrderingSystem.Models;

namespace OnlineOrderingSystem.Database
{
    /// <summary>
    /// Data access layer for customer/user data using Entity Framework Core
    /// </summary>
    public class UserDataAccess
    {
        /// <summary>
        /// Creates a new customer in the database
        /// </summary>
        /// <param name="customer">The customer to create</param>
        /// <returns>The created customer with its generated ID</returns>
        public Customer CreateCustomer(Customer customer)
        {
            using (var context = new OrderingDbContext())
            {
                context.Customers.Add(customer);
                context.SaveChanges();
                return customer;
            }
        }

        /// <summary>
        /// Retrieves a customer by ID
        /// </summary>
        /// <param name="customerId">The customer ID</param>
        /// <returns>The customer or null if not found</returns>
        public Customer? GetCustomerById(int customerId)
        {
            using (var context = new OrderingDbContext())
            {
                return context.Customers
                    .Include(c => c.OrderHistory)
                    .FirstOrDefault(c => c.CustomerID == customerId);
            }
        }

        /// <summary>
        /// Authenticates a customer by email and password
        /// </summary>
        /// <param name="email">The email address</param>
        /// <param name="password">The password</param>
        /// <returns>The authenticated customer or null if authentication fails</returns>
        public Customer? AuthenticateCustomer(string email, string password)
        {
            using (var context = new OrderingDbContext())
            {
                var customer = context.Customers
                    .Include(c => c.OrderHistory)
                    .FirstOrDefault(c => c.Email == email && c.Password == password);
                
                return customer;
            }
        }

        /// <summary>
        /// Retrieves a customer by email address
        /// </summary>
        /// <param name="email">The email address</param>
        /// <returns>The customer or null if not found</returns>
        public Customer? GetCustomerByEmail(string email)
        {
            using (var context = new OrderingDbContext())
            {
                return context.Customers
                    .Include(c => c.OrderHistory)
                    .FirstOrDefault(c => c.Email==email);
            }
        }

        /// <summary>
        /// Retrieves all customers
        /// </summary>
        /// <returns>List of all customers</returns>
        public List<Customer> GetAllCustomers()
        {
            using (var context = new OrderingDbContext())
            {
                return context.Customers
                    .Include(c => c.OrderHistory)
                    .ToList();
            }
        }

        /// <summary>
        /// Updates an existing customer
        /// </summary>
        /// <param name="customer">The customer to update</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool UpdateCustomer(Customer customer)
        {
            using (var context = new OrderingDbContext())
            {
                context.Customers.Update(customer);
                return context.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// Deletes a customer by ID
        /// </summary>
        /// <param name="customerId">The customer ID to delete</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool DeleteCustomer(int customerId)
        {
            using (var context = new OrderingDbContext())
            {
                var customer = context.Customers.Find(customerId);
                if (customer != null)
                {
                    context.Customers.Remove(customer);
                    return context.SaveChanges() > 0;
                }
                return false;
            }
        }

        /// <summary>
        /// Validates customer login credentials
        /// </summary>
        /// <param name="email">The email address</param>
        /// <param name="password">The password (in a real app, this would be hashed)</param>
        /// <returns>The customer if credentials are valid, null otherwise</returns>
        public Customer? ValidateLogin(string email, string password)
        {
            using (var context = new OrderingDbContext())
            {
                // Note: In a real application, passwords would be hashed and salted
                // This is a simplified implementation for demonstration
                var customer = context.Customers
                    .FirstOrDefault(c => c.Email.ToLower() == email.ToLower());
                
                // In a real app, you would verify hashed password here
                if (customer != null)
                {
                    // For demonstration, we'll just return the customer
                    // In reality, you'd compare password hashes
                    return customer;
                }
                
                return null;
            }
        }

        /// <summary>
        /// Checks if an email address is already registered
        /// </summary>
        /// <param name="email">The email address to check</param>
        /// <returns>True if email exists, false otherwise</returns>
        public bool EmailExists(string email)
        {
            using (var context = new OrderingDbContext())
            {
                return context.Customers
                    .Any(c => c.Email.ToLower() == email.ToLower());
            }
        }

        /// <summary>
        /// Searches customers by name
        /// </summary>
        /// <param name="searchTerm">The search term</param>
        /// <returns>List of customers matching the search term</returns>
        public List<Customer> SearchCustomers(string searchTerm)
        {
            using (var context = new OrderingDbContext())
            {
                return context.Customers
                    .Where(c => c.Name.Contains(searchTerm) || 
                               c.Email.Contains(searchTerm))
                    .ToList();
            }
        }

        /// <summary>
        /// Gets customers who placed orders within a date range
        /// </summary>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <returns>List of customers who ordered in the date range</returns>
        public List<Customer> GetCustomersWithOrdersInDateRange(DateTime startDate, DateTime endDate)
        {
            using (var context = new OrderingDbContext())
            {
                return context.Customers
                    .Include(c => c.OrderHistory)
                    .Where(c => c.OrderHistory.Any(o => o.OrderDate >= startDate && o.OrderDate <= endDate))
                    .ToList();
            }
        }

        /// <summary>
        /// Gets top customers by total order value
        /// </summary>
        /// <param name="topCount">Number of top customers to return</param>
        /// <returns>List of top customers by spending</returns>
        public List<Customer> GetTopCustomersBySpending(int topCount = 10)
        {
            using (var context = new OrderingDbContext())
            {
                return context.Customers
                    .Include(c => c.OrderHistory)
                    .OrderByDescending(c => c.OrderHistory.Sum(o => o.TotalAmount))
                    .Take(topCount)
                    .ToList();
            }
        }

        /// <summary>
        /// Updates customer's preferred payment method
        /// </summary>
        /// <param name="customerId">The customer ID</param>
        /// <param name="paymentMethod">The new preferred payment method</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool UpdateCustomerPaymentMethod(int customerId, string paymentMethod)
        {
            using (var context = new OrderingDbContext())
            {
                var customer = context.Customers.Find(customerId);
                if (customer != null)
                {
                    customer.PreferredPaymentMethod = paymentMethod;
                    context.Customers.Update(customer);
                    return context.SaveChanges() > 0;
                }
                return false;
            }
        }
    }
} 