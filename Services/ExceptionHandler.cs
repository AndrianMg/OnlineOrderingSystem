using System;
using System.Windows.Forms;
using OnlineOrderingSystem.Exceptions;
using OnlineOrderingSystem.Models;

namespace OnlineOrderingSystem.Services
{
    /// <summary>
    /// Centralized exception handler for the application
    /// Demonstrates proper custom exception handling and user feedback
    /// </summary>
    public static class ExceptionHandler
    {
        /// <summary>
        /// Handles exceptions and provides appropriate user feedback
        /// </summary>
        /// <param name="ex">The exception to handle</param>
        /// <param name="context">Context where the exception occurred</param>
        /// <param name="showUserMessage">Whether to show a message to the user</param>
        public static void Handle(Exception ex, string context, bool showUserMessage = true)
        {
            // Log the exception
            GlobalClass.GetInstance().LogEvent("Error", $"[{context}] {ex.Message}");

            // Handle custom exceptions with specific logic
            switch (ex)
            {
                case InvalidOrderException orderEx:
                    HandleOrderException(orderEx, context, showUserMessage);
                    break;
                case PaymentProcessingException paymentEx:
                    HandlePaymentException(paymentEx, context, showUserMessage);
                    break;
                case InsufficientInventoryException inventoryEx:
                    HandleInventoryException(inventoryEx, context, showUserMessage);
                    break;
                case AuthenticationException authEx:
                    HandleAuthenticationException(authEx, context, showUserMessage);
                    break;
                default:
                    HandleGenericException(ex, context, showUserMessage);
                    break;
            }
        }

        /// <summary>
        /// Handles order-related exceptions
        /// </summary>
        private static void HandleOrderException(InvalidOrderException ex, string context, bool showUserMessage)
        {
            var logMessage = $"Order Error [ID: {ex.OrderId}, Status: {ex.OrderStatus}]: {ex.Message}";
            GlobalClass.GetInstance().LogEvent("OrderError", logMessage);

            if (showUserMessage)
            {
                var userMessage = GetOrderUserMessage(ex);
                MessageBox.Show(userMessage, "Order Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Handles payment-related exceptions
        /// </summary>
        private static void HandlePaymentException(PaymentProcessingException ex, string context, bool showUserMessage)
        {
            var logMessage = $"Payment Error [Method: {ex.PaymentMethod}, Amount: {ex.Amount:C}]: {ex.Message}";
            GlobalClass.GetInstance().LogEvent("PaymentError", logMessage);

            if (showUserMessage)
            {
                var userMessage = GetPaymentUserMessage(ex);
                MessageBox.Show(userMessage, "Payment Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles inventory-related exceptions
        /// </summary>
        private static void HandleInventoryException(InsufficientInventoryException ex, string context, bool showUserMessage)
        {
            var logMessage = $"Inventory Error [Item: {ex.ItemId}, Requested: {ex.RequestedQuantity}, Available: {ex.AvailableQuantity}]: {ex.Message}";
            GlobalClass.GetInstance().LogEvent("InventoryError", logMessage);

            if (showUserMessage)
            {
                var userMessage = GetInventoryUserMessage(ex);
                MessageBox.Show(userMessage, "Inventory Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Handles authentication-related exceptions
        /// </summary>
        private static void HandleAuthenticationException(AuthenticationException ex, string context, bool showUserMessage)
        {
            var logMessage = $"Authentication Error [User: {ex.Username}, Reason: {ex.Reason}]: {ex.Message}";
            GlobalClass.GetInstance().LogEvent("AuthError", logMessage);

            if (showUserMessage)
            {
                var userMessage = GetAuthenticationUserMessage(ex);
                MessageBox.Show(userMessage, "Authentication Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles generic exceptions
        /// </summary>
        private static void HandleGenericException(Exception ex, string context, bool showUserMessage)
        {
            var logMessage = $"Generic Error [{context}]: {ex.Message}";
            GlobalClass.GetInstance().LogEvent("GenericError", logMessage);

            if (showUserMessage)
            {
                var userMessage = GetGenericUserMessage(ex);
                MessageBox.Show(userMessage, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Gets user-friendly message for order exceptions
        /// </summary>
        private static string GetOrderUserMessage(InvalidOrderException ex)
        {
            return $"Order Error: {ex.Message}\n\nPlease check your order details and try again.";
        }

        /// <summary>
        /// Gets user-friendly message for payment exceptions
        /// </summary>
        private static string GetPaymentUserMessage(PaymentProcessingException ex)
        {
            return $"Payment Error: {ex.Message}\n\nPlease verify your payment information and try again.";
        }

        /// <summary>
        /// Gets user-friendly message for inventory exceptions
        /// </summary>
        private static string GetInventoryUserMessage(InsufficientInventoryException ex)
        {
            return $"Inventory Error: {ex.Message}\n\nPlease reduce the quantity or choose a different item.";
        }

        /// <summary>
        /// Gets user-friendly message for authentication exceptions
        /// </summary>
        private static string GetAuthenticationUserMessage(AuthenticationException ex)
        {
            return $"Authentication Error: {ex.Message}\n\nPlease check your credentials and try again.";
        }

        /// <summary>
        /// Gets user-friendly message for generic exceptions
        /// </summary>
        private static string GetGenericUserMessage(Exception ex)
        {
            return $"An unexpected error occurred: {ex.Message}\n\nPlease try again or contact support if the problem persists.";
        }

        /// <summary>
        /// Determines if an exception is recoverable
        /// </summary>
        public static bool IsRecoverable(Exception ex)
        {
            return ex is InvalidOrderException || ex is InsufficientInventoryException;
        }

        /// <summary>
        /// Gets recovery suggestion for an exception
        /// </summary>
        public static string GetRecoverySuggestion(Exception ex)
        {
            return ex switch
            {
                InvalidOrderException => "Please review and correct your order details.",
                InsufficientInventoryException => "Please reduce the quantity or choose alternative items.",
                PaymentProcessingException => "Please verify your payment information and try again.",
                AuthenticationException => "Please check your username and password.",
                _ => "Please try again or contact support for assistance."
            };
        }
    }
}

