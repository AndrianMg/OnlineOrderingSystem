using System;
using OnlineOrderingSystem.Models;

namespace OnlineOrderingSystem.Models
{
    /// <summary>
    /// Cash payment processing
    /// </summary>
    public class Cash : Payment
    {
        public decimal AmountTendered { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal ChangeDue => AmountTendered - TotalAmount;

        public Cash()
        {
            PaymentMethod = "Cash";
            PaymentStatus = "Pending";
        }

        public override void ProcessPayment()
        {
            if (AmountTendered < TotalAmount)
            {
                throw new InvalidOperationException("Insufficient amount tendered");
            }

            PaymentStatus = "Completed";
            PaymentDate = DateTime.Now;
            Console.WriteLine($"Cash payment processed. Amount: {TotalAmount:C}, Tendered: {AmountTendered:C}, Change: {ChangeDue:C}");
        }

        public override bool ValidatePayment()
        {
            return AmountTendered >= TotalAmount && TotalAmount > 0;
        }
    }
} 