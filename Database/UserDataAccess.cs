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
    }
} 