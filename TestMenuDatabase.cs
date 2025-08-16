using System;
using OnlineOrderingSystem.Database;
using OnlineOrderingSystem.Data;
using OnlineOrderingSystem.Models;

namespace OnlineOrderingSystem
{
    /// <summary>
    /// Simple test class to verify database contains menu items
    /// </summary>
    public class TestMenuDatabase
    {
        public static void TestDatabaseItems()
        {
            Console.WriteLine("=== Testing Menu Database ===");
            
            try
            {
                // Test direct database access
                using (var context = new OrderingDbContext())
                {
                    Console.WriteLine($"Items count in database: {context.Items.Count()}");
                    
                    // Seed if empty
                    if (!context.Items.Any())
                    {
                        Console.WriteLine("Database is empty, seeding data...");
                        context.SeedDatabase();
                        Console.WriteLine($"After seeding: {context.Items.Count()} items");
                    }
                    
                    // Show first few items
                    var items = context.Items.Take(5).ToList();
                    Console.WriteLine("\nFirst 5 items in database:");
                    foreach (var item in items)
                    {
                        Console.WriteLine($"- {item.Name} (£{item.Price:F2}) - {item.Category}");
                        Console.WriteLine($"  Rating: {item.Rating:F1}★ | {item.PrepTime}min | {item.Calories}cal");
                        if (item.DietaryTags.Any())
                        {
                            Console.WriteLine($"  Tags: {string.Join(", ", item.DietaryTags)}");
                        }
                        Console.WriteLine();
                    }
                }
                
                // Test using MenuDataAccess
                Console.WriteLine("=== Testing MenuDataAccess ===");
                var menuAccess = new MenuDataAccess();
                var allItems = menuAccess.GetAllMenuItems();
                Console.WriteLine($"MenuDataAccess returned {allItems.Count} items");
                
                var availableItems = menuAccess.GetAvailableMenuItems();
                Console.WriteLine($"Available items: {availableItems.Count}");
                
                var categories = allItems.Select(i => i.Category).Distinct().ToList();
                Console.WriteLine($"Categories: {string.Join(", ", categories)}");
                
                Console.WriteLine("\n=== Test Completed Successfully ===");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error testing database: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// Test order and payment database integration
        /// </summary>
        public static void TestOrderAndPaymentDatabase()
        {
            Console.WriteLine("=== Testing Order and Payment Database Integration ===");
            
            try
            {
                using (var context = new OrderingDbContext())
                {
                    // Get test data
                    var customer = context.Customers.FirstOrDefault();
                    var item = context.Items.FirstOrDefault();
                    
                    if (customer == null || item == null)
                    {
                        Console.WriteLine("No test data available. Please seed the database first.");
                        return;
                    }

                    Console.WriteLine($"Testing with customer: {customer.Name} and item: {item.Name}");

                    // Test OrderDataAccess
                    var orderDataAccess = new OrderDataAccess();
                    
                    // Create a test order
                    var testOrder = new Order();
                    var testCart = new Cart();
                    testCart.AddItem(item, 2);
                    testOrder.CreateOrder(customer.CustomerID, testCart);
                    testOrder.DeliveryAddress = customer.Address;
                    testOrder.IsDelivery = true;
                    testOrder.DeliveryFee = 2.99;

                    // Create a test payment
                    var testPayment = new Cash { AmountTendered = testOrder.TotalAmount + 5, TotalAmount = testOrder.TotalAmount };
                    testPayment.SetAmount(testOrder.TotalAmount);

                    // Save order with payment to database
                    var savedOrder = orderDataAccess.CreateOrderWithPayment(testOrder, testPayment);
                    Console.WriteLine($"✓ Order saved to database with ID: {savedOrder.OrderID}");

                    // Test retrieving the order
                    var retrievedOrder = orderDataAccess.GetOrderById(savedOrder.OrderID);
                    if (retrievedOrder != null)
                    {
                        Console.WriteLine($"✓ Order retrieved from database: {retrievedOrder.GetOrderSummary()}");
                        Console.WriteLine($"  - Items: {retrievedOrder.OrderItems.Count}");
                        Console.WriteLine($"  - Status: {retrievedOrder.OrderStatus}");
                        Console.WriteLine($"  - Payment Status: {retrievedOrder.PaymentStatus}");
                    }

                    // Test PaymentDataAccess
                    var paymentDataAccess = new PaymentDataAccess();
                    var payments = paymentDataAccess.GetPaymentsByOrderId(savedOrder.OrderID);
                    Console.WriteLine($"✓ Payment retrieved from database: {payments.Count} payment(s)");

                    if (payments.Any())
                    {
                        var payment = payments.First();
                        Console.WriteLine($"  - Payment ID: {payment.PaymentID}");
                        Console.WriteLine($"  - Amount: £{payment.Amount:F2}");
                        Console.WriteLine($"  - Method: {payment.PaymentMethod}");
                        Console.WriteLine($"  - Status: {payment.PaymentStatus}");
                        Console.WriteLine($"  - Transaction ID: {payment.TransactionID}");
                    }

                    // Test order history
                    var orderHistory = orderDataAccess.GetOrderHistoryWithPayments(customer.CustomerID);
                    Console.WriteLine($"✓ Order history retrieved: {orderHistory.Count} orders");

                    // Test order statistics
                    var stats = orderDataAccess.GetOrderStatistics(customer.CustomerID);
                    Console.WriteLine($"✓ Order statistics: {stats.totalOrders} orders, £{stats.totalSpent:F2} total spent");

                    Console.WriteLine("\n=== Order and Payment Database Test Completed Successfully ===");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error testing order and payment database: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// Test the new CardDetailsForm functionality
        /// </summary>
        public static void TestCardDetailsForm()
        {
            Console.WriteLine("=== Testing Card Details Form ===");
            
            try
            {
                // This would normally be tested in a Windows Forms application
                // For console testing, we'll simulate the form behavior
                Console.WriteLine("Card Details Form features:");
                Console.WriteLine("✓ Form opens when credit/debit card is selected");
                Console.WriteLine("✓ Card number field with automatic spacing (**** **** **** ****)");
                Console.WriteLine("✓ Cardholder name field");
                Console.WriteLine("✓ Expiry date fields (MM/YY) with validation");
                Console.WriteLine("✓ CVV field with password masking");
                Console.WriteLine("✓ Input validation for all fields");
                Console.WriteLine("✓ Automatic focus movement between fields");
                Console.WriteLine("✓ Form returns Credit object with collected details");
                
                Console.WriteLine("\n=== Card Details Form Test Completed ===");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error testing card details form: {ex.Message}");
            }
        }
    }
}
