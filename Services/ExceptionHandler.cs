using System;
using System.Windows.Forms;
using OnlineOrderingSystem.Exceptions;
using OnlineOrderingSystem.Models;

namespace OnlineOrderingSystem.Services
{
    /// <summary>
    /// Centralized exception handler demonstrating proper error handling patterns.
    /// 
    /// This class shows how to implement:
    /// - Strategy Pattern for different exception types
    /// - User-friendly error messages
    /// - Centralized logging and error management
    /// 
    /// Note: This is demonstration code - not actively used in the current application.
    /// </summary>
    public static class ExceptionHandler
    {
        /// <summary>
        /// Main entry point for exception handling.
        /// 
        /// Routes different exception types to appropriate handlers using Strategy Pattern.
        /// </summary>
        /// <param name="ex">The exception to handle.</param>
        /// <param name="context">Context where the exception occurred.</param>
        /// <param name="showUserMessage">Whether to display a user-friendly error message.</param>
        public static void Handle(Exception ex, string context, bool showUserMessage = true)
        {
            // Validate input parameters
            if (ex == null)
                throw new ArgumentNullException(nameof(ex), "Exception cannot be null");
            
            if (string.IsNullOrWhiteSpace(context))
                context = "Unknown Context";

            // Step 1: Log the exception with context information for debugging and monitoring
            // This ensures all exceptions are captured in the application log regardless of type
            GlobalClass.GetInstance().LogEvent("Error", $"[{context}] {ex.Message}");

            // Step 2: Route to appropriate handler using Strategy Pattern
            // Pattern matching provides type-safe exception handling with specific logic for each type
            switch (ex)
            {
                case InvalidOrderException orderEx:
                    // Handle order-specific exceptions (invalid quantities, missing items, etc.)
                    HandleOrderException(orderEx, context, showUserMessage);
                    break;
                    
                case PaymentProcessingException paymentEx:
                    // Handle payment-related exceptions (card declined, insufficient funds, etc.)
                    HandlePaymentException(paymentEx, context, showUserMessage);
                    break;
                    
                case InsufficientInventoryException inventoryEx:
                    // Handle inventory-related exceptions (out of stock, quantity limits, etc.)
                    HandleInventoryException(inventoryEx, context, showUserMessage);
                    break;
                    
                case AuthenticationException authEx:
                    // Handle authentication exceptions (invalid credentials, expired sessions, etc.)
                    HandleAuthenticationException(authEx, context, showUserMessage);
                    break;
                    
                default:
                    // Handle all other exceptions (system errors, unexpected exceptions, etc.)
                    HandleGenericException(ex, context, showUserMessage);
                    break;
            }
        }

        /// <summary>
        /// Handles order-related exceptions.
        /// </summary>
        /// <param name="ex">The InvalidOrderException to handle.</param>
        /// <param name="context">The context where the exception occurred.</param>
        /// <param name="showUserMessage">Whether to display a user-friendly error dialog.</param>
        private static void HandleOrderException(InvalidOrderException ex, string context, bool showUserMessage)
        {
            // Create detailed log message with order-specific information
            // This helps with debugging and provides context for support staff
            var logMessage = $"Order Error [ID: {ex.OrderId}, Status: {ex.OrderStatus}]: {ex.Message}";
            GlobalClass.GetInstance().LogEvent("OrderError", logMessage);

            // Display user-friendly message if requested
            // Warning icon indicates the issue is recoverable with user action
            if (showUserMessage)
            {
                var userMessage = GetOrderUserMessage(ex);
                MessageBox.Show(userMessage, "Order Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Handles payment processing exceptions.
        /// </summary>
        /// <param name="ex">The PaymentProcessingException to handle.</param>
        /// <param name="context">The context where the exception occurred.</param>
        /// <param name="showUserMessage">Whether to display a user-friendly error dialog.</param>
        private static void HandlePaymentException(PaymentProcessingException ex, string context, bool showUserMessage)
        {
            // Create detailed log message with payment information for debugging
            // Note: In production, sensitive payment data should be masked or excluded
            var logMessage = $"Payment Error [Method: {ex.PaymentMethod}, Amount: {ex.Amount:C}]: {ex.Message}";
            GlobalClass.GetInstance().LogEvent("PaymentError", logMessage);

            // Display user-friendly message if requested
            // Error icon indicates this is a critical issue requiring immediate attention
            if (showUserMessage)
            {
                var userMessage = GetPaymentUserMessage(ex);
                MessageBox.Show(userMessage, "Payment Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles inventory-related exceptions.
        /// </summary>
        /// <param name="ex">The InsufficientInventoryException to handle.</param>
        /// <param name="context">The context where the exception occurred.</param>
        /// <param name="showUserMessage">Whether to display a user-friendly error dialog.</param>
        private static void HandleInventoryException(InsufficientInventoryException ex, string context, bool showUserMessage)
        {
            // Create detailed log message with inventory information for debugging
            // This helps track inventory issues and supports business analysis
            var logMessage = $"Inventory Error [Item: {ex.ItemId}, Requested: {ex.RequestedQuantity}, Available: {ex.AvailableQuantity}]: {ex.Message}";
            GlobalClass.GetInstance().LogEvent("InventoryError", logMessage);

            // Display user-friendly message if requested
            // Warning icon indicates the issue is recoverable with user action
            if (showUserMessage)
            {
                var userMessage = GetInventoryUserMessage(ex);
                MessageBox.Show(userMessage, "Inventory Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Handles authentication-related exceptions.
        /// </summary>
        /// <param name="ex">The AuthenticationException to handle.</param>
        /// <param name="context">The context where the exception occurred.</param>
        /// <param name="showUserMessage">Whether to display a user-friendly error dialog.</param>
        private static void HandleAuthenticationException(AuthenticationException ex, string context, bool showUserMessage)
        {
            // Create detailed log message with authentication information for security monitoring
            // Note: In production, consider logging security events to a separate secure log
            var logMessage = $"Authentication Error [User: {ex.Username}, Reason: {ex.Reason}]: {ex.Message}";
            GlobalClass.GetInstance().LogEvent("AuthError", logMessage);

            // Display user-friendly message if requested
            // Error icon indicates this is a security-related issue
            if (showUserMessage)
            {
                var userMessage = GetAuthenticationUserMessage(ex);
                MessageBox.Show(userMessage, "Authentication Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles generic exceptions that don't fit into specific categories.
        /// </summary>
        /// <param name="ex">The generic Exception to handle.</param>
        /// <param name="context">The context where the exception occurred.</param>
        /// <param name="showUserMessage">Whether to display a user-friendly error dialog.</param>
        private static void HandleGenericException(Exception ex, string context, bool showUserMessage)
        {
            // Create log message for generic exceptions
            // Include stack trace for debugging purposes
            var logMessage = $"Generic Error [{context}]: {ex.Message}";
            GlobalClass.GetInstance().LogEvent("GenericError", logMessage);

            // Display user-friendly message if requested
            // Error icon indicates this is a system-level issue
            if (showUserMessage)
            {
                var userMessage = GetGenericUserMessage(ex);
                MessageBox.Show(userMessage, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Generates user-friendly error message for order-related exceptions.
        /// </summary>
        /// <param name="ex">The InvalidOrderException containing error details.</param>
        /// <returns>A user-friendly error message.</returns>
        private static string GetOrderUserMessage(InvalidOrderException ex)
        {
            return $"Order Error: {ex.Message}\n\nPlease check your order details and try again.";
        }

        /// <summary>
        /// Generates user-friendly error message for payment-related exceptions.
        /// </summary>
        /// <param name="ex">The PaymentProcessingException containing error details.</param>
        /// <returns>A user-friendly error message.</returns>
        private static string GetPaymentUserMessage(PaymentProcessingException ex)
        {
            return $"Payment Error: {ex.Message}\n\nPlease verify your payment information and try again.";
        }

        /// <summary>
        /// Generates user-friendly error message for inventory-related exceptions.
        /// </summary>
        /// <param name="ex">The InsufficientInventoryException containing inventory details.</param>
        /// <returns>A user-friendly error message.</returns>
        private static string GetInventoryUserMessage(InsufficientInventoryException ex)
        {
            return $"Inventory Error: {ex.Message}\n\nPlease reduce the quantity or choose a different item.";
        }

        /// <summary>
        /// Generates user-friendly error message for authentication-related exceptions.
        /// </summary>
        /// <param name="ex">The AuthenticationException containing authentication details.</param>
        /// <returns>A user-friendly error message.</returns>
        private static string GetAuthenticationUserMessage(AuthenticationException ex)
        {
            return $"Authentication Error: {ex.Message}\n\nPlease check your credentials and try again.";
        }

        /// <summary>
        /// Generates user-friendly error message for generic exceptions.
        /// </summary>
        /// <param name="ex">The generic Exception containing error details.</param>
        /// <returns>A user-friendly error message.</returns>
        private static string GetGenericUserMessage(Exception ex)
        {
            return $"An unexpected error occurred: {ex.Message}\n\nPlease try again or contact support if the problem persists.";
        }

        /// <summary>
        /// Determines whether an exception represents a recoverable error condition.
        /// </summary>
        /// <param name="ex">The exception to evaluate for recoverability.</param>
        /// <returns>True if the exception is recoverable through user action; otherwise, false.</returns>
        public static bool IsRecoverable(Exception ex)
        {
            return ex is InvalidOrderException || ex is InsufficientInventoryException;
        }

        /// <summary>
        /// Provides specific recovery suggestions based on the exception type.
        /// </summary>
        /// <param name="ex">The exception for which to provide recovery suggestions.</param>
        /// <returns>A specific recovery suggestion message for the given exception type.</returns>
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

