using System;

namespace OnlineOrderingSystem.Models
{
    /// <summary>
    /// Represents check payment processing
    /// </summary>
    public class Check : Payment
    {
        public string ChequeNumber { get; set; } = string.Empty;
        public string BankName { get; set; } = string.Empty;
        public bool IsCleared { get; set; }

        /// <summary>
        /// Processes the check payment
        /// </summary>
        public override void ProcessPayment()
        {
            if (ValidatePayment())
            {
                PaymentStatus = "Pending";
                PaymentDate = DateTime.Now;
                Console.WriteLine($"Check payment processed. Check number: {ChequeNumber}");
            }
            else
            {
                PaymentStatus = "Failed";
                Console.WriteLine("Check payment failed validation.");
            }
        }

        /// <summary>
        /// Validates the check payment
        /// </summary>
        /// <returns>True if check is valid, false otherwise</returns>
        public bool ValidateCheque()
        {
            if (string.IsNullOrWhiteSpace(ChequeNumber))
            {
                Console.WriteLine("Check number is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(BankName))
            {
                Console.WriteLine("Bank name is required.");
                return false;
            }

            if (GetAmount() <= 0)
            {
                Console.WriteLine("Invalid check amount.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates the payment (implements abstract method)
        /// </summary>
        /// <returns>True if payment is valid, false otherwise</returns>
        public override bool ValidatePayment()
        {
            return ValidateCheque();
        }

        /// <summary>
        /// Gets the payment details for check payment
        /// </summary>
        /// <returns>Check payment details</returns>
        public override string GetPaymentDetails()
        {
            return $"Check Payment - Check Number: {ChequeNumber}, Bank: {BankName}, Amount: ${GetAmount():F2}, Cleared: {IsCleared}";
        }

        /// <summary>
        /// Updates the payment status
        /// </summary>
        /// <param name="newStatus">The new status</param>
        public void UpdatePaymentStatus(string newStatus)
        {
            PaymentStatus = newStatus;
            if (newStatus == "Cleared")
            {
                IsCleared = true;
            }
        }
    }
} 