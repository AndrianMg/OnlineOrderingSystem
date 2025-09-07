using System;
using System.Collections.Generic;
using OnlineOrderingSystem.Models;

namespace OnlineOrderingSystem.Services
{
    /// <summary>
    /// Core service interface for ordering operations.
    /// 
    /// Demonstrates:
    /// - Interface Segregation Principle
    /// - Dependency Injection patterns
    /// - Strategy Pattern for payments
    /// 
    /// Note: This is demonstration code - not actively used in the current application.
    /// </summary>
    public interface IOrderingService
    {
        /// <summary>
        /// Retrieves all available menu items from the system.
        /// </summary>
        /// <returns>A list of all available menu items.</returns>
        List<Item> GetAllItems();

        /// <summary>
        /// Retrieves menu items filtered by a specific category.
        /// </summary>
        /// <param name="category">The category name to filter by.</param>
        /// <returns>A list of available items in the specified category.</returns>
        List<Item> GetItemsByCategory(string category);

        /// <summary>
        /// Creates and places a new order in the system.
        /// </summary>
        /// <param name="customerID">The unique identifier of the customer placing the order.</param>
        /// <param name="itemIDs">List of item identifiers to include in the order.</param>
        /// <returns>A complete Order object with all details populated.</returns>
        Order PlaceOrder(int customerID, List<int> itemIDs);

        /// <summary>
        /// Retrieves customer information by unique identifier.
        /// </summary>
        /// <param name="customerID">The unique identifier of the customer to retrieve.</param>
        /// <returns>Customer object with complete information, or null if customer not found.</returns>
        Customer? GetCustomerById(int customerID);

        /// <summary>
        /// Processes a payment transaction for an order.
        /// </summary>
        /// <param name="payment">The payment object containing all payment details.</param>
        /// <returns>True if payment was successfully processed; false if payment failed.</returns>
        bool ProcessPayment(Payment payment);

        /// <summary>
        /// Retrieves complete order history for a specific customer.
        /// </summary>
        /// <param name="customerID">The unique identifier of the customer.</param>
        /// <returns>List of orders for the specified customer.</returns>
        List<Order> GetOrderHistory(int customerID);

        /// <summary>
        /// Retrieves list of available payment methods supported by the system.
        /// </summary>
        /// <returns>List of available payment method names.</returns>
        List<string> GetAvailablePaymentMethods();

        /// <summary>
        /// Creates a payment object based on the specified payment method and amount.
        /// </summary>
        /// <param name="paymentMethod">The payment method type.</param>
        /// <param name="amount">The payment amount.</param>
        /// <returns>A configured payment object ready for processing.</returns>
        Payment CreatePayment(string paymentMethod, double amount);
    }
} 