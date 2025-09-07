using System;
using OnlineOrderingSystem.Models;

namespace OnlineOrderingSystem.Models
{
    /// <summary>
    /// Credit card payment processing implementation.
    /// 
    /// Demonstrates:
    /// - Concrete implementation of Payment abstract class
    /// - Credit card validation logic
    /// - Polymorphism in payment processing
    /// </summary>
    public class Credit : Payment
    {
        public string CardNumber { get; set; } = string.Empty;
        public string CardHolderName { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
        public int CVV { get; set; }

        public Credit()
        {
            PaymentMethod = "Credit";
            PaymentStatus = "Pending";
        }

        public override void ProcessPayment()
        {
            if (!ValidatePayment())
            {
                throw new InvalidOperationException("Credit card validation failed");
            }

            // Simulate credit card processing
            PaymentStatus = "Completed";
            PaymentDate = DateTime.Now;
            Console.WriteLine($"Credit card payment processed for {CardHolderName}. Amount: {Amount:C}");
        }

        public override bool ValidatePayment()
        {
            if (string.IsNullOrWhiteSpace(CardNumber) || CardNumber.Length < 13 || CardNumber.Length > 19)
                return false;

            if (string.IsNullOrWhiteSpace(CardHolderName))
                return false;

            if (ExpiryDate < DateTime.Now)
                return false;

            if (CVV < 100 || CVV > 999)
                return false;

            return Amount > 0;
        }
    }
} 