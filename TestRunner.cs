using System;
using OnlineOrderingSystem.Data;

namespace OnlineOrderingSystem
{
    /// <summary>
    /// Simple test runner to execute only unit tests without GUI
    /// </summary>
    public class TestRunner
    {
        /// <summary>
        /// Main entry point for running tests only
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Unit Test Runner ===");
            Console.WriteLine("Running tests only (no GUI)...");
            Console.WriteLine();

            try
            {
                // Initialize database for testing
                using (var context = new OrderingDbContext())
                {
                    context.Database.EnsureCreated();
                    if (!context.Customers.Any())
                    {
                        context.SeedDatabase();
                    }
                }

                Console.WriteLine("🔧 Database initialized for testing...");
                Console.WriteLine();

                // Run comprehensive database testing
                Console.WriteLine("RUNNING COMPREHENSIVE TESTING SUITE");
                Console.WriteLine(new string('=', 50));
                
                DatabaseDemo.RunDatabaseTest();

                Console.WriteLine();
                Console.WriteLine("RUNNING MENU DATABASE TESTING");
                Console.WriteLine(new string('=', 50));
                
                TestMenuDatabase.TestDatabaseItems();

                Console.WriteLine();
                Console.WriteLine(new string('=', 50));
                Console.WriteLine("TESTING COMPLETED SUCCESSFULLY!");
                Console.WriteLine(new string('=', 50));
                
                Console.WriteLine();
                Console.WriteLine("This demonstrates:");
                Console.WriteLine("✅ Unit Testing Implementation");
                Console.WriteLine("✅ Integration Testing");
                Console.WriteLine("✅ System Testing");
                Console.WriteLine("✅ AAA Testing Pattern");
                Console.WriteLine("✅ Test Results and Metrics");
                
                Console.WriteLine();
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ERROR during testing: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                Console.WriteLine();
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}
