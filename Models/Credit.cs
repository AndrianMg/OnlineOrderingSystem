using System;

namespace OnlineOrderingSystem.Models
{
    /// <summary>
    /// Represents credit card payment processing
    /// </summary>
    public class Credit : Payment
    {
        public string CardNumber { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
        public string CardHolderName { get; set; } = string.Empty;
        public int CVV { get; set; }
        public bool Authorized { get; set; }

        /// <summary>
        /// Processes the credit card payment
        /// </summary>
        public override void ProcessPayment()
        {
            if (ValidatePayment())
            {
                // Simulate credit card authorization
                Authorized = true;
                PaymentStatus = "Completed";
                PaymentDate = DateTime.Now;
                Console.WriteLine($"Credit card payment processed. Card ending in {CardNumber[^4..]}");
            }
            else
            {
                Authorized = false;
                PaymentStatus = "Failed";
                Console.WriteLine("Credit card payment failed validation.");
            }
        }

        /// <summary>
        /// Validates the credit card
        /// </summary>
        /// <returns>True if card is valid, false otherwise</returns>
        public bool ValidateCard()
        {
            if (string.IsNullOrWhiteSpace(CardNumber) || CardNumber.Length < 13)
            {
                Console.WriteLine("Invalid card number.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(CardHolderName))
            {
                Console.WriteLine("Card holder name is required.");
                return false;
            }

            if (ExpiryDate < DateTime.Now)
            {
                Console.WriteLine("Card has expired.");
                return false;
            }

            if (CVV < 100 || CVV > 9999)
            {
                Console.WriteLine("Invalid CVV.");
                return false;
            }

            if (GetAmount() <= 0)
            {
                Console.WriteLine("Invalid payment amount.");
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
            return ValidateCard();
        }

        /// <summary>
        /// Gets the payment details for credit card payment
        /// </summary>
        /// <returns>Credit card payment details</returns>
        public override string GetPaymentDetails()
        {
            string maskedCardNumber = CardNumber.Length >= 4 ? $"****-****-****-{CardNumber[^4..]}" : "****";
            return $"Credit Card Payment - Card: {maskedCardNumber}, Holder: {CardHolderName}, Amount: ${GetAmount():F2}, Authorized: {Authorized}";
        }

        /// <summary>
        /// Sends payment confirmation
        /// </summary>
        public void SendPaymentConfirmation()
        {
            if (Authorized)
            {
                Console.WriteLine($"Payment confirmation sent to {CardHolderName} for ${GetAmount():F2}");
            }
        }

        /// <summary>
        /// Masks the card number for security
        /// </summary>
        /// <returns>Masked card number</returns>
        public string GetMaskedCardNumber()
        {
            if (string.IsNullOrEmpty(CardNumber) || CardNumber.Length < 4)
                return "****";

            return $"****-****-****-{CardNumber[^4..]}";
        }
    }
} 