using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OnlineOrderingSystem.Data;
using OnlineOrderingSystem.Models;

namespace OnlineOrderingSystem.Database
{
    /// <summary>
    /// Data access layer for payments using Entity Framework Core
    /// </summary>
    public class PaymentDataAccess
    {
        /// <summary>
        /// Creates a new payment in the database
        /// </summary>
        /// <param name="payment">The payment to create</param>
        /// <returns>The created payment with its generated ID</returns>
        public PaymentEntity CreatePayment(PaymentEntity payment)
        {
            using (var context = new OrderingDbContext())
            {
                context.PaymentEntities.Add(payment);
                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving payment: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                    }
                    throw;
                }
                return payment;
            }
        }

        /// <summary>
        /// Retrieves a payment by ID
        /// </summary>
        /// <param name="paymentId">The payment ID</param>
        /// <returns>The payment or null if not found</returns>
        public PaymentEntity? GetPaymentById(int paymentId)
        {
            using (var context = new OrderingDbContext())
            {
                return context.PaymentEntities.FirstOrDefault(p => p.PaymentID == paymentId);
            }
        }

        /// <summary>
        /// Retrieves all payments for a specific order
        /// </summary>
        /// <param name="orderId">The order ID</param>
        /// <returns>List of payments for the order</returns>
        public List<PaymentEntity> GetPaymentsByOrderId(int orderId)
        {
            using (var context = new OrderingDbContext())
            {
                return context.PaymentEntities
                    .Where(p => p.OrderID == orderId)
                    .OrderByDescending(p => p.PaymentDate)
                    .ToList();
            }
        }

        /// <summary>
        /// Retrieves all payments for a specific customer
        /// </summary>
        /// <param name="customerId">The customer ID</param>
        /// <returns>List of payments for the customer</returns>
        public List<PaymentEntity> GetPaymentsByCustomerId(int customerId)
        {
            using (var context = new OrderingDbContext())
            {
                return context.PaymentEntities
                    .Where(p => p.CustomerID == customerId)
                    .OrderByDescending(p => p.PaymentDate)
                    .ToList();
            }
        }

        /// <summary>
        /// Retrieves all payments with a specific status
        /// </summary>
        /// <param name="status">The payment status to filter by</param>
        /// <returns>List of payments with the specified status</returns>
        public List<PaymentEntity> GetPaymentsByStatus(string status)
        {
            using (var context = new OrderingDbContext())
            {
                return context.PaymentEntities
                    .Where(p => p.PaymentStatus.ToLower() == status.ToLower())
                    .OrderByDescending(p => p.PaymentDate)
                    .ToList();
            }
        }

        /// <summary>
        /// Updates an existing payment
        /// </summary>
        /// <param name="payment">The payment to update</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool UpdatePayment(PaymentEntity payment)
        {
            using (var context = new OrderingDbContext())
            {
                context.PaymentEntities.Update(payment);
                return context.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// Updates the status of a payment
        /// </summary>
        /// <param name="paymentId">The payment ID</param>
        /// <param name="newStatus">The new status</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool UpdatePaymentStatus(int paymentId, string newStatus)
        {
            using (var context = new OrderingDbContext())
            {
                var payment = context.PaymentEntities.FirstOrDefault(p => p.PaymentID == paymentId);
                if (payment != null)
                {
                    payment.PaymentStatus = newStatus;
                    payment.PaymentDate = DateTime.Now;
                    return context.SaveChanges() > 0;
                }
                return false;
            }
        }

        /// <summary>
        /// Retrieves payment statistics for a customer
        /// </summary>
        /// <param name="customerId">The customer ID</param>
        /// <returns>Tuple containing total payments, total amount, and average payment</returns>
        public (int totalPayments, double totalAmount, double averagePayment) GetPaymentStatistics(int customerId)
        {
            using (var context = new OrderingDbContext())
            {
                var payments = context.PaymentEntities
                    .Where(p => p.CustomerID == customerId && p.PaymentStatus == "Completed")
                    .ToList();

                var totalPayments = payments.Count;
                var totalAmount = payments.Sum(p => p.Amount);
                var averagePayment = totalPayments > 0 ? totalAmount / totalPayments : 0;

                return (totalPayments, totalAmount, averagePayment);
            }
        }

        /// <summary>
        /// Retrieves payment history for a customer with order details
        /// </summary>
        /// <param name="customerId">The customer ID</param>
        /// <returns>List of payments with order information</returns>
        public List<PaymentWithOrder> GetPaymentHistoryWithOrders(int customerId)
        {
            using (var context = new OrderingDbContext())
            {
                var query = from payment in context.PaymentEntities
                           join order in context.Orders on payment.OrderID equals order.OrderID
                           where payment.CustomerID == customerId
                           orderby payment.PaymentDate descending
                           select new PaymentWithOrder
                           {
                               Payment = payment,
                               Order = order
                           };

                return query.ToList();
            }
        }

        /// <summary>
        /// Deletes a payment (for administrative purposes)
        /// </summary>
        /// <param name="paymentId">The payment ID to delete</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool DeletePayment(int paymentId)
        {
            using (var context = new OrderingDbContext())
            {
                var payment = context.PaymentEntities.FirstOrDefault(p => p.PaymentID == paymentId);
                if (payment != null)
                {
                    context.PaymentEntities.Remove(payment);
                    return context.SaveChanges() > 0;
                }
                return false;
            }
        }
    }

    /// <summary>
    /// DTO for payment with order information
    /// </summary>
    public class PaymentWithOrder
    {
        public PaymentEntity Payment { get; set; } = new();
        public Order Order { get; set; } = new();
    }
}
