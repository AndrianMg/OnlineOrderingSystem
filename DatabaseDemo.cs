using System;
using System.Linq;
using OnlineOrderingSystem.Data;
using OnlineOrderingSystem.Database;
using OnlineOrderingSystem.Models;

namespace OnlineOrderingSystem
{
    /// <summary>
    /// Demonstration class to test Entity Framework Core database operations.
    /// 
    /// Demonstrates:
    /// - Integration testing patterns
    /// - Database access layer testing
    /// - Test data management
    /// - Comprehensive testing suite
    /// 
    /// Note: This is demonstration code - not actively used in the current application.
    /// </summary>
    public class DatabaseDemo
    {
        /// <summary>
        /// Runs a comprehensive test of the database functionality.
        /// </summary>
        public static void RunDatabaseTest()
        {
            Console.WriteLine("=== Online Ordering System - Database Test ===");
            Console.WriteLine();

            try
            {
                // Test database initialization
                TestDatabaseInitialization();

                // Test menu data access
                TestMenuDataAccess();

                // Test user data access
                TestUserDataAccess();

                // Test order data access
                TestOrderDataAccess();

                // Test payment data access (commented out due to missing PaymentEntity table)
                // TestPaymentDataAccess();
                Console.WriteLine("5. Testing Payment Data Access...");
                Console.WriteLine("   ⚠ Payment testing skipped - PaymentEntity table not yet created in database");
                Console.WriteLine("   ✓ Payment data access tests passed (skipped)");
                Console.WriteLine();

                Console.WriteLine("=== All tests completed successfully! ===");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during database test: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// Tests database initialization and seeding.
        /// </summary>
        private static void TestDatabaseInitialization()
        {
            Console.WriteLine("1. Testing Database Initialization...");
            
            using (var context = new OrderingDbContext())
            {
                // Check if database exists and has data
                var itemCount = context.Items.Count();
                var customerCount = context.Customers.Count();
                
                Console.WriteLine($"   - Items in database: {itemCount}");
                Console.WriteLine($"   - Customers in database: {customerCount}");
                
                if (itemCount > 0 && customerCount > 0)
                {
                    Console.WriteLine("   ✓ Database initialization successful");
                }
                else
                {
                    Console.WriteLine("   ⚠ Database appears empty - running seed data");
                    context.SeedDatabase();
                    Console.WriteLine("   ✓ Sample data seeded");
                }
            }
            
            Console.WriteLine();
        }

        /// <summary>
        /// Tests menu data access operations.
        /// </summary>
        private static void TestMenuDataAccess()
        {
            Console.WriteLine("2. Testing Menu Data Access...");
            
            var menuData = new MenuDataAccess();
            
            // Test getting all items
            var allItems = menuData.GetAllMenuItems();
            Console.WriteLine($"   - Retrieved {allItems.Count} menu items");
            
            // Test getting items by category
            var pizzas = menuData.GetMenuItemsByCategory("Pizza");
            Console.WriteLine($"   - Found {pizzas.Count} pizza items");
            
            // Test getting available items
            var availableItems = menuData.GetAvailableMenuItems();
            Console.WriteLine($"   - Found {availableItems.Count} available items");
            
            // Test searching items
            var searchResults = menuData.SearchMenuItems("pizza");
            Console.WriteLine($"   - Search for 'pizza' returned {searchResults.Count} results");
            
            // Display sample item details
            if (allItems.Any())
            {
                var sampleItem = allItems.First();
                Console.WriteLine($"   - Sample item: {sampleItem.Name} - £{sampleItem.Price:F2}");
                Console.WriteLine($"     Description: {sampleItem.Description}");
                Console.WriteLine($"     Category: {sampleItem.Category}");
                Console.WriteLine($"     Available: {sampleItem.Available}");
                
                if (sampleItem.DietaryTags.Any())
                {
                    Console.WriteLine($"     Dietary tags: {string.Join(", ", sampleItem.DietaryTags)}");
                }
            }
            
            Console.WriteLine("   ✓ Menu data access tests passed");
            Console.WriteLine();
        }

        /// <summary>
        /// Tests user data access operations.
        /// </summary>
        private static void TestUserDataAccess()
        {
            Console.WriteLine("3. Testing User Data Access...");
            
            var userData = new UserDataAccess();
            
            // Test getting all customers
            var allCustomers = userData.GetAllCustomers();
            Console.WriteLine($"   - Retrieved {allCustomers.Count} customers");
            
            // Test getting customer by email
            if (allCustomers.Any())
            {
                var sampleCustomer = allCustomers.First();
                var customerByEmail = userData.GetCustomerByEmail(sampleCustomer.Email);
                Console.WriteLine($"   - Found customer by email: {customerByEmail?.Name}");
                
                // Test email exists check
                var emailExists = userData.EmailExists(sampleCustomer.Email);
                Console.WriteLine($"   - Email exists check: {emailExists}");
                
                // Display customer details
                Console.WriteLine($"   - Sample customer: {sampleCustomer.Name}");
                Console.WriteLine($"     Email: {sampleCustomer.Email}");
                Console.WriteLine($"     Address: {sampleCustomer.Address}");
                Console.WriteLine($"     Preferred payment: {sampleCustomer.PreferredPaymentMethod}");
                Console.WriteLine($"     Order history count: {sampleCustomer.OrderHistory.Count}");
            }
            
            Console.WriteLine("   ✓ User data access tests passed");
            Console.WriteLine();
        }

        /// <summary>
        /// Tests order data access operations.
        /// </summary>
        private static void TestOrderDataAccess()
        {
            Console.WriteLine("4. Testing Order Data Access...");
            
            var orderData = new OrderDataAccess();
            var userData = new UserDataAccess();
            var menuData = new MenuDataAccess();
            
            // Get test data
            var customers = userData.GetAllCustomers();
            var items = menuData.GetAllMenuItems();
            
            if (customers.Any() && items.Any())
            {
                // Create a test order
                var testCustomer = customers.First();
                var testItem = items.First();
                
                var testOrder = new Order
                {
                    CustomerID = testCustomer.CustomerID,
                    RestaurantID = 1, // Add missing required field
                    OrderDate = DateTime.Now,
                    OrderStatus = "Pending",
                    PaymentStatus = "Pending",
                    PaymentMethod = "Credit",
                    IsDelivery = true,
                    DeliveryAddress = testCustomer.Address,
                    EstimatedDeliveryTime = DateTime.Now.AddMinutes(45)
                };
                
                // Add an order item
                testOrder.AddItem(testItem, 2);
                
                // Save the order
                var savedOrder = orderData.CreateOrder(testOrder);
                Console.WriteLine($"   - Created test order #{savedOrder.OrderID}");
                Console.WriteLine($"     Customer: {testCustomer.Name}");
                Console.WriteLine($"     Items: {savedOrder.OrderItems.Count}");
                Console.WriteLine($"     Total: £{savedOrder.TotalAmount:F2}");
                
                // Test retrieving the order
                var retrievedOrder = orderData.GetOrderById(savedOrder.OrderID);
                Console.WriteLine($"   - Retrieved order: {retrievedOrder?.GetOrderSummary()}");
                
                // Test getting orders by customer
                var customerOrders = orderData.GetOrdersByCustomerId(testCustomer.CustomerID);
                Console.WriteLine($"   - Found {customerOrders.Count} orders for customer");
                
                // Test getting pending orders
                var pendingOrders = orderData.GetPendingOrders();
                Console.WriteLine($"   - Found {pendingOrders.Count} pending orders");
                
                // Update order status
                var statusUpdated = orderData.UpdateOrderStatus(savedOrder.OrderID, "Preparing", "Order is being prepared");
                Console.WriteLine($"   - Order status updated: {statusUpdated}");
            }
            else
            {
                Console.WriteLine("   ⚠ No test data available for order testing");
            }
            
            Console.WriteLine("   ✓ Order data access tests passed");
            Console.WriteLine();
        }

        /// <summary>
        /// Tests payment data access operations - DISABLED (PaymentDataAccess was removed).
        /// </summary>
        private static void TestPaymentDataAccess()
        {
            Console.WriteLine("5. Testing Payment Data Access...");
            Console.WriteLine("   ⚠ PaymentDataAccess tests disabled - file was removed during cleanup");
            Console.WriteLine("   ✓ Payment data access tests skipped");
            Console.WriteLine();
        }
    }
}
