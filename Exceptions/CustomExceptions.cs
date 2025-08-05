using System;

namespace OnlineOrderingSystem.Exceptions
{
    /// <summary>
    /// Custom exception for invalid order operations
    /// Demonstrates custom exception handling and business rule validation
    /// </summary>
    public class InvalidOrderException : Exception
    {
        public int OrderId { get; }
        public string OrderStatus { get; }

        public InvalidOrderException(string message, int orderId, string orderStatus) 
            : base(message)
        {
            OrderId = orderId;
            OrderStatus = orderStatus;
        }

        public InvalidOrderException(string message, int orderId, string orderStatus, Exception innerException) 
            : base(message, innerException)
        {
            OrderId = orderId;
            OrderStatus = orderStatus;
        }
    }

    /// <summary>
    /// Custom exception for payment processing errors
    /// Demonstrates domain-specific exception handling
    /// </summary>
    public class PaymentProcessingException : Exception
    {
        public string PaymentMethod { get; }
        public double Amount { get; }

        public PaymentProcessingException(string message, string paymentMethod, double amount) 
            : base(message)
        {
            PaymentMethod = paymentMethod;
            Amount = amount;
        }

        public PaymentProcessingException(string message, string paymentMethod, double amount, Exception innerException) 
            : base(message, innerException)
        {
            PaymentMethod = paymentMethod;
            Amount = amount;
        }
    }

    /// <summary>
    /// Custom exception for inventory management
    /// Demonstrates business rule validation through exceptions
    /// </summary>
    public class InsufficientInventoryException : Exception
    {
        public int ItemId { get; }
        public int RequestedQuantity { get; }
        public int AvailableQuantity { get; }

        public InsufficientInventoryException(string message, int itemId, int requestedQuantity, int availableQuantity) 
            : base(message)
        {
            ItemId = itemId;
            RequestedQuantity = requestedQuantity;
            AvailableQuantity = availableQuantity;
        }
    }

    /// <summary>
    /// Custom exception for authentication and authorization
    /// Demonstrates security-related exception handling
    /// </summary>
    public class AuthenticationException : Exception
    {
        public string Username { get; }
        public string Reason { get; }

        public AuthenticationException(string message, string username, string reason) 
            : base(message)
        {
            Username = username;
            Reason = reason;
        }
    }
} 