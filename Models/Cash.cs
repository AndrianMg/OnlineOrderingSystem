using System;

namespace OnlineOrderingSystem.Models
{
    /// <summary>
    /// Represents cash payment processing
    /// </summary>
    public class Cash : Payment
    {
        public double AmountTendered { get; set; }
        public double TotalAmount { get; set; }
        public double ChangeDue { get; set; }

        /// <summary>
        /// Calculates the change due for cash payment
        /// </summary>
        /// <returns>The change amount</returns>
        public double CalculateChange()
        {
            ChangeDue = AmountTendered - TotalAmount;
            return ChangeDue;
        }

        /// <summary>
        /// Validates the cash payment
        /// </summary>
        /// <returns>True if payment is valid, false otherwise</returns>
        public override bool ValidatePayment()
        {
            if (AmountTendered < TotalAmount)
            {
                Console.WriteLine("Insufficient cash tendered.");
                return false;
            }

            if (AmountTendered <= 0)
            {
                Console.WriteLine("Invalid amount tendered.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Processes the cash payment
        /// </summary>
        public override void ProcessPayment()
        {
            if (ValidatePayment())
            {
                PaymentStatus = "Completed";
                PaymentDate = DateTime.Now;
                CalculateChange();
                Console.WriteLine($"Cash payment processed. Change due: ${ChangeDue:F2}");
            }
            else
            {
                PaymentStatus = "Failed";
                Console.WriteLine("Cash payment failed validation.");
            }
        }

        /// <summary>
        /// Gets the payment details for cash payment
        /// </summary>
        /// <returns>Cash payment details</returns>
        public override string GetPaymentDetails()
        {
            return $"Cash Payment - Amount Tendered: ${AmountTendered:F2}, Total: ${TotalAmount:F2}, Change: ${ChangeDue:F2}";
        }
    }
} 