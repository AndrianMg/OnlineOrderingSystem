using System;
using System.Windows.Forms;
using OnlineOrderingSystem.Exceptions;
using OnlineOrderingSystem.Models;

namespace OnlineOrderingSystem.Services
{
    /// <summary>
    /// Centralized exception handler for the Online Ordering System application.
    /// 
    /// This class implements a comprehensive exception handling strategy that:
    /// - Provides centralized error management across the entire application
    /// - Demonstrates proper custom exception handling patterns
    /// - Offers user-friendly error messages while maintaining detailed logging
    /// - Implements the Strategy Pattern for different exception types
    /// - Supports both recoverable and non-recoverable error scenarios
    /// 
    /// Design Patterns Used:
    /// - Strategy Pattern: Different handling strategies for different exception types
    /// - Template Method Pattern: Consistent error handling flow with specific implementations
    /// - Singleton Pattern: Global access through static methods
    /// 
    /// Exception Categories Handled:
    /// - Order-related exceptions (InvalidOrderException)
    /// - Payment processing exceptions (PaymentProcessingException)
    /// - Inventory management exceptions (InsufficientInventoryException)
    /// - Authentication exceptions (AuthenticationException)
    /// - Generic system exceptions (Exception)
    /// 
    /// Features:
    /// - Automatic logging of all exceptions with context information
    /// - User-friendly error messages that hide technical details
    /// - Recovery suggestions for recoverable exceptions
    /// - Configurable user notification (can be disabled for background operations)
    /// - Detailed error categorization for monitoring and debugging
    /// </summary>
    public static class ExceptionHandler
    {
        /// <summary>
        /// Main entry point for exception handling in the application.
        /// 
        /// This method implements the Strategy Pattern to route different exception types
        /// to their appropriate handlers. It ensures consistent error processing across
        /// the entire application while providing specific handling for each exception category.
        /// 
        /// Process Flow:
        /// 1. Log the exception with context information for debugging
        /// 2. Determine the exception type using pattern matching
        /// 3. Route to appropriate specialized handler
        /// 4. Each handler manages logging, user notification, and recovery suggestions
        /// 
        /// Usage Examples:
        /// <code>
        /// try 
        /// {
        ///     // Some operation that might throw an exception
        ///     ProcessOrder(order);
        /// }
        /// catch (Exception ex)
        /// {
        ///     ExceptionHandler.Handle(ex, "Order Processing", true);
        /// }
        /// </code>
        /// </summary>
        /// <param name="ex">The exception to handle. Cannot be null.</param>
        /// <param name="context">Context where the exception occurred (e.g., "Order Processing", "Payment Validation"). 
        /// Used for logging and debugging purposes.</param>
        /// <param name="showUserMessage">Whether to display a user-friendly error message.
        /// Set to false for background operations or when handling exceptions programmatically.</param>
        /// <exception cref="ArgumentNullException">Thrown when ex parameter is null.</exception>
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
        /// Handles order-related exceptions with specialized logging and user feedback.
        /// 
        /// This method processes InvalidOrderException instances, which typically occur when:
        /// - Order validation fails (invalid quantities, missing required fields)
        /// - Order state transitions are invalid (canceling a completed order)
        /// - Order business rules are violated (minimum order amounts, delivery restrictions)
        /// 
        /// The method provides detailed logging for debugging while presenting
        /// user-friendly messages that guide the user toward resolution.
        /// </summary>
        /// <param name="ex">The InvalidOrderException to handle. Contains order-specific details.</param>
        /// <param name="context">The context where the exception occurred (e.g., "Order Validation").</param>
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
        /// Handles payment processing exceptions with specialized logging and user feedback.
        /// 
        /// This method processes PaymentProcessingException instances, which typically occur when:
        /// - Credit card validation fails (invalid card number, expired date, insufficient funds)
        /// - Payment gateway communication errors (network issues, service unavailable)
        /// - Payment method restrictions (unsupported payment types, regional limitations)
        /// - Security validation failures (CVV mismatch, fraud detection triggers)
        /// 
        /// Payment exceptions are critical as they directly affect revenue and customer trust.
        /// The method ensures sensitive payment information is not exposed in user messages
        /// while providing sufficient detail for troubleshooting.
        /// </summary>
        /// <param name="ex">The PaymentProcessingException to handle. Contains payment-specific details.</param>
        /// <param name="context">The context where the exception occurred (e.g., "Payment Processing").</param>
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
        /// Handles inventory-related exceptions with specialized logging and user feedback.
        /// 
        /// This method processes InsufficientInventoryException instances, which typically occur when:
        /// - Requested item quantity exceeds available stock
        /// - Items become unavailable between cart addition and checkout
        /// - Inventory synchronization issues between systems
        /// - Items are marked as discontinued or temporarily unavailable
        /// 
        /// Inventory exceptions are often recoverable by suggesting alternatives or
        /// allowing users to adjust quantities. The method provides clear guidance
        /// to help users complete their orders successfully.
        /// </summary>
        /// <param name="ex">The InsufficientInventoryException to handle. Contains inventory-specific details.</param>
        /// <param name="context">The context where the exception occurred (e.g., "Inventory Check").</param>
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
        /// Handles authentication-related exceptions with specialized logging and user feedback.
        /// 
        /// This method processes AuthenticationException instances, which typically occur when:
        /// - Invalid username or password credentials
        /// - Account is locked due to multiple failed login attempts
        /// - Session has expired and requires re-authentication
        /// - Account is disabled or suspended by administrators
        /// - Password has expired and requires reset
        /// 
        /// Authentication exceptions are security-sensitive and require careful handling
        /// to prevent information disclosure while providing helpful guidance to users.
        /// The method balances security with usability.
        /// </summary>
        /// <param name="ex">The AuthenticationException to handle. Contains authentication-specific details.</param>
        /// <param name="context">The context where the exception occurred (e.g., "User Login").</param>
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
        /// 
        /// This method serves as a catch-all for unexpected exceptions that don't
        /// inherit from the custom exception types. These typically include:
        /// - System-level exceptions (OutOfMemoryException, StackOverflowException)
        /// - Third-party library exceptions
        /// - Unexpected runtime errors
        /// - Exceptions from external services
        /// 
        /// Generic exceptions are often more serious and may indicate system issues
        /// that require immediate attention. The method provides basic error handling
        /// while ensuring the application remains stable.
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
        /// 
        /// This method creates a clear, actionable message that helps users understand
        /// what went wrong with their order and what they can do to fix it. The message
        /// avoids technical jargon and focuses on user actions.
        /// </summary>
        /// <param name="ex">The InvalidOrderException containing error details.</param>
        /// <returns>A user-friendly error message with actionable guidance.</returns>
        private static string GetOrderUserMessage(InvalidOrderException ex)
        {
            return $"Order Error: {ex.Message}\n\nPlease check your order details and try again.";
        }

        /// <summary>
        /// Generates user-friendly error message for payment-related exceptions.
        /// 
        /// This method creates a clear message that guides users through payment issues
        /// without exposing sensitive payment information. The message focuses on
        /// verification steps users can take to resolve the issue.
        /// </summary>
        /// <param name="ex">The PaymentProcessingException containing error details.</param>
        /// <returns>A user-friendly error message with payment guidance.</returns>
        private static string GetPaymentUserMessage(PaymentProcessingException ex)
        {
            return $"Payment Error: {ex.Message}\n\nPlease verify your payment information and try again.";
        }

        /// <summary>
        /// Generates user-friendly error message for inventory-related exceptions.
        /// 
        /// This method creates a helpful message that guides users to resolve inventory
        /// issues by suggesting alternative actions like reducing quantities or choosing
        /// different items.
        /// </summary>
        /// <param name="ex">The InsufficientInventoryException containing inventory details.</param>
        /// <returns>A user-friendly error message with inventory guidance.</returns>
        private static string GetInventoryUserMessage(InsufficientInventoryException ex)
        {
            return $"Inventory Error: {ex.Message}\n\nPlease reduce the quantity or choose a different item.";
        }

        /// <summary>
        /// Generates user-friendly error message for authentication-related exceptions.
        /// 
        /// This method creates a security-conscious message that helps users resolve
        /// authentication issues without revealing sensitive account information.
        /// The message provides general guidance without being too specific.
        /// </summary>
        /// <param name="ex">The AuthenticationException containing authentication details.</param>
        /// <returns>A user-friendly error message with authentication guidance.</returns>
        private static string GetAuthenticationUserMessage(AuthenticationException ex)
        {
            return $"Authentication Error: {ex.Message}\n\nPlease check your credentials and try again.";
        }

        /// <summary>
        /// Generates user-friendly error message for generic exceptions.
        /// 
        /// This method creates a general error message for unexpected system errors.
        /// It provides reassurance to users while encouraging them to try again or
        /// contact support for persistent issues.
        /// </summary>
        /// <param name="ex">The generic Exception containing error details.</param>
        /// <returns>A user-friendly error message with general guidance.</returns>
        private static string GetGenericUserMessage(Exception ex)
        {
            return $"An unexpected error occurred: {ex.Message}\n\nPlease try again or contact support if the problem persists.";
        }

        /// <summary>
        /// Determines whether an exception represents a recoverable error condition.
        /// 
        /// Recoverable exceptions are those that can be resolved by user action or
        /// application logic without requiring system intervention. This method helps
        /// the application decide whether to allow retry attempts or provide recovery options.
        /// 
        /// Recoverable Exception Types:
        /// - InvalidOrderException: User can correct order details
        /// - InsufficientInventoryException: User can adjust quantities or choose alternatives
        /// 
        /// Non-Recoverable Exception Types:
        /// - PaymentProcessingException: May require payment method change or support
        /// - AuthenticationException: May require account reset or support
        /// - Generic exceptions: Often indicate system issues requiring intervention
        /// </summary>
        /// <param name="ex">The exception to evaluate for recoverability.</param>
        /// <returns>True if the exception is recoverable through user action; otherwise, false.</returns>
        public static bool IsRecoverable(Exception ex)
        {
            return ex is InvalidOrderException || ex is InsufficientInventoryException;
        }

        /// <summary>
        /// Provides specific recovery suggestions based on the exception type.
        /// 
        /// This method implements the Strategy Pattern to provide tailored recovery
        /// guidance for different exception types. The suggestions are designed to
        /// help users take specific actions to resolve the error condition.
        /// 
        /// The method serves as a centralized source of recovery guidance, ensuring
        /// consistency across the application and making it easy to update recovery
        /// strategies as the application evolves.
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

