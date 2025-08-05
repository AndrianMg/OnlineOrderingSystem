using System;
using System.Collections.Generic;
using OnlineOrderingSystem.Models;

namespace OnlineOrderingSystem.Services
{
    /// <summary>
    /// Interface for the ordering service
    /// Demonstrates dependency injection and interface segregation principle
    /// </summary>
    public interface IOrderingService
    {
        /// <summary>
        /// Gets all available items
        /// </summary>
        /// <returns>List of all items</returns>
        List<Item> GetAllItems();

        /// <summary>
        /// Gets items by category
        /// </summary>
        /// <param name="category">The category to filter by</param>
        /// <returns>List of items in the category</returns>
        List<Item> GetItemsByCategory(string category);

        /// <summary>
        /// Places an order
        /// </summary>
        /// <param name="customerID">The customer ID</param>
        /// <param name="itemIDs">List of item IDs to order</param>
        /// <returns>The created order</returns>
        Order PlaceOrder(int customerID, List<int> itemIDs);

        /// <summary>
        /// Gets customer by ID
        /// </summary>
        /// <param name="customerID">The customer ID</param>
        /// <returns>The customer or null if not found</returns>
        Customer? GetCustomerById(int customerID);

        /// <summary>
        /// Processes a payment
        /// </summary>
        /// <param name="payment">The payment to process</param>
        /// <returns>True if payment was successful, false otherwise</returns>
        bool ProcessPayment(Payment payment);

        /// <summary>
        /// Gets order history for a customer
        /// </summary>
        /// <param name="customerID">The customer ID</param>
        /// <returns>List of orders for the customer</returns>
        List<Order> GetOrderHistory(int customerID);

        /// <summary>
        /// Gets available payment methods
        /// </summary>
        /// <returns>List of available payment methods</returns>
        List<string> GetAvailablePaymentMethods();

        /// <summary>
        /// Creates a payment object based on payment method
        /// </summary>
        /// <param name="paymentMethod">The payment method</param>
        /// <param name="amount">The payment amount</param>
        /// <returns>The payment object</returns>
        Payment CreatePayment(string paymentMethod, double amount);
    }
} 