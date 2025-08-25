using System;
using OnlineOrderingSystem.Models;

namespace OnlineOrderingSystem.Models
{
    /// <summary>
    /// Check payment processing
    /// </summary>
    public class Check : Payment
    {
        public string ChequeNumber { get; set; } = string.Empty;
        public string BankName { get; set; } = string.Empty;
        public DateTime CheckDate { get; set; }

        public Check()
        {
            PaymentMethod = "Check";
            PaymentStatus = "Pending";
            CheckDate = DateTime.Now;
        }

        public override void ProcessPayment()
        {
            if (!ValidatePayment())
            {
                throw new InvalidOperationException("Check validation failed");
            }

            // Simulate check processing
            PaymentStatus = "Completed";
            PaymentDate = DateTime.Now;
            Console.WriteLine($"Check payment processed. Check #{ChequeNumber} from {BankName}. Amount: {Amount:C}");
        }

        public override bool ValidatePayment()
        {
            if (string.IsNullOrWhiteSpace(ChequeNumber))
                return false;

            if (string.IsNullOrWhiteSpace(BankName))
                return false;

            if (CheckDate > DateTime.Now)
                return false;

            return Amount > 0;
        }
    }
} 