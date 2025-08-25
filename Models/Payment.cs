using System;

namespace OnlineOrderingSystem.Models
{
    /// <summary>
    /// Abstract base class for payment processing
    /// </summary>
    public abstract class Payment
    {
        public int PaymentID { get; set; }
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string PaymentStatus { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; }
        public string PaymentDetails { get; set; } = string.Empty;
        public string TransactionID { get; set; } = string.Empty;

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
            return $"Payment ID: {PaymentID}, Amount: {Amount:C}, Status: {PaymentStatus}, Method: {PaymentMethod}";
        }

        /// <summary>
        /// Validates the payment
        /// </summary>
        /// <returns>True if payment is valid, false otherwise</returns>
        public abstract bool ValidatePayment();
    }

    /// <summary>
    /// Concrete payment entity for database storage
    /// </summary>
    public class PaymentEntity
    {
        public int PaymentID { get; set; }
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string PaymentStatus { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; }
        public string PaymentDetails { get; set; } = string.Empty;
        public string TransactionID { get; set; } = string.Empty;
        public string CardNumber { get; set; } = string.Empty;
        public string CardHolderName { get; set; } = string.Empty;
        public DateTime? ExpiryDate { get; set; }
        public int? CVV { get; set; }
        public string ChequeNumber { get; set; } = string.Empty;
        public string BankName { get; set; } = string.Empty;
        public decimal AmountTendered { get; set; }

        /// <summary>
        /// Creates a PaymentEntity from a Payment object
        /// </summary>
        /// <param name="payment">The payment object</param>
        /// <param name="orderId">The order ID</param>
        /// <param name="customerId">The customer ID</param>
        /// <returns>A new PaymentEntity</returns>
        public static PaymentEntity FromPayment(Payment payment, int orderId, int customerId)
        {
            var entity = new PaymentEntity
            {
                OrderID = orderId,
                CustomerID = customerId,
                Amount = payment.Amount,
                PaymentMethod = payment.PaymentMethod,
                PaymentStatus = payment.PaymentStatus,
                PaymentDate = DateTime.Now,
                TransactionID = Guid.NewGuid().ToString("N")[..16].ToUpper(),
                PaymentDetails = payment.GetPaymentDetails()
            };

            // Set specific payment method details
            switch (payment)
            {
                case Credit credit:
                    entity.CardNumber = credit.CardNumber;
                    entity.CardHolderName = credit.CardHolderName;
                    entity.ExpiryDate = credit.ExpiryDate;
                    entity.CVV = credit.CVV;
                    break;
                case Check check:
                    entity.ChequeNumber = check.ChequeNumber;
                    entity.BankName = check.BankName;
                    break;
                case Cash cash:
                    entity.AmountTendered = cash.AmountTendered;
                    break;
            }

            return entity;
        }
    }
}



