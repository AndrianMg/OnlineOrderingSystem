using System;

namespace OnlineOrderingSystem.Models
{
    /// <summary>
    /// Abstract base class for payment processing
    /// </summary>
    public abstract class Payment
    {
        protected int PaymentID { get; set; }
        protected int OrderID { get; set; }
        protected int CustomerID { get; set; }
        protected double Amount { get; set; }
        protected string PaymentMethod { get; set; } = string.Empty;
        protected string PaymentStatus { get; set; } = string.Empty;
        protected DateTime PaymentDate { get; set; }
        protected string PaymentDetails { get; set; } = string.Empty;
        protected string TransactionID { get; set; } = string.Empty;

        /// <summary>
        /// Processes the payment
        /// </summary>
        public abstract void ProcessPayment();

        /// <summary>
        /// Refunds the payment
        /// </summary>
        public virtual void RefundPayment()
        {
            PaymentStatus = "Refunded";
            Console.WriteLine($"Payment {PaymentID} has been refunded.");
        }

        /// <summary>
        /// Gets the payment details
        /// </summary>
        /// <returns>Payment details as string</returns>
        public virtual string GetPaymentDetails()
        {
            return $"Payment ID: {PaymentID}, Amount: ${Amount:F2}, Status: {PaymentStatus}, Method: {PaymentMethod}";
        }

        /// <summary>
        /// Validates the payment
        /// </summary>
        /// <returns>True if payment is valid, false otherwise</returns>
        public abstract bool ValidatePayment();

        /// <summary>
        /// Sets the payment amount
        /// </summary>
        /// <param name="amount">The amount to set</param>
        public void SetAmount(double amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Amount cannot be negative");
            }
            Amount = amount;
        }

        /// <summary>
        /// Gets the payment amount
        /// </summary>
        /// <returns>The payment amount</returns>
        public double GetAmount()
        {
            return Amount;
        }

        /// <summary>
        /// Sets the payment status
        /// </summary>
        /// <param name="status">The status to set</param>
        public void SetPaymentStatus(string status)
        {
            PaymentStatus = status;
        }

        /// <summary>
        /// Gets the payment status
        /// </summary>
        /// <returns>The payment status</returns>
        public string GetPaymentStatus()
        {
            return PaymentStatus;
        }
    }
} 