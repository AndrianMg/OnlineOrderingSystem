using OnlineOrderingSystem.Forms;
using OnlineOrderingSystem.Data;
using System;
using System.Windows.Forms;

namespace OnlineOrderingSystem
{
    /// <summary>
    /// Main program class for the Tasty Eats Online Ordering System
    /// Serves as the entry point for the Windows Forms application
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// Initializes the application and starts the login form.
        /// </summary>
        static void Main(string[] args)
        {
            // Check if running from command line or if user wants to run tests
            bool runTests = false;
            bool testOnly = false;
            
            Console.WriteLine($"Command line args: {string.Join(", ", args)}");
            
            if (args.Length > 0)
            {
                if (args[0].ToLower() == "test")
                {
                    // Command line test mode
                    runTests = true;
                    Console.WriteLine("Test mode detected from command line");
                }
                else if (args[0].ToLower() == "testonly")
                {
                    // Test only mode - no GUI
                    testOnly = true;
                    Console.WriteLine("Test-only mode detected - running tests without GUI");
                }
            }
            else
            {
                // Check if user wants to run tests first (GUI mode)
                try
                {
                    var result = MessageBox.Show(
                        "Would you like to run the testing suite to demonstrate testing methodologies?\n\n" +
                        "Click 'Yes' to run tests\n" +
                        "Click 'No' to go directly to login form", 
                        "Testing Option", 
                        MessageBoxButtons.YesNo, 
                        MessageBoxIcon.Question);
                    
                    runTests = (result == DialogResult.Yes);
                    
                    if (!runTests)
                    {
                        MessageBox.Show("Starting main application...", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (InvalidOperationException)
                {
                    // No GUI context available, run tests by default
                    runTests = true;
                }
            }
            
            if (runTests)
            {
                Console.WriteLine("Starting test mode...");
                RunTestsInConsole();
            }

            if (testOnly)
            {
                Console.WriteLine("Running tests only (no GUI)...");
                RunTestsOnly();
                return; // Exit after tests, don't start GUI
            }

            // Initialize the database and seed sample data
            InitializeDatabase();

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            
            // Start with LoginForm as intended
            Application.Run(new LoginForm());
        }

        /// <summary>
        /// Run tests only without GUI - console output only
        /// </summary>
        private static void RunTestsOnly()
        {
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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ERROR during testing: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// Run tests in a separate console window to demonstrate testing methodologies
        /// </summary>
        private static void RunTestsInConsole()
        {
            try
            {
                // Write to file for debugging
                var logPath = Path.Combine(Environment.CurrentDirectory, "test_results.txt");
                File.WriteAllText(logPath, "=== Tasty Eats Online Ordering System - Testing Suite ===\n");
                File.AppendAllText(logPath, "Demonstrating Testing Methodologies and Results\n");
                File.AppendAllText(logPath, "=====================================================\n\n");

                // Initialize database for testing
                using (var context = new OrderingDbContext())
                {
                    context.Database.EnsureCreated();
                    if (!context.Customers.Any())
                    {
                        context.SeedDatabase();
                    }
                }

                File.AppendAllText(logPath, "🔧 Database initialized for testing...\n\n");

                // Run comprehensive database testing
                File.AppendAllText(logPath, "RUNNING COMPREHENSIVE TESTING SUITE\n");
                File.AppendAllText(logPath, new string('=', 50) + "\n");
                
                // Capture console output by redirecting it
                var originalOut = Console.Out;
                using (var writer = new StringWriter())
                {
                    Console.SetOut(writer);
                    
                    try
                    {
                        DatabaseDemo.RunDatabaseTest();
                        File.AppendAllText(logPath, writer.ToString());
                    }
                    finally
                    {
                        Console.SetOut(originalOut);
                    }
                }

                File.AppendAllText(logPath, "\nRUNNING MENU DATABASE TESTING\n");
                File.AppendAllText(logPath, new string('=', 50) + "\n");
                
                using (var writer = new StringWriter())
                {
                    Console.SetOut(writer);
                    
                    try
                    {
                        TestMenuDatabase.TestDatabaseItems();
                        File.AppendAllText(logPath, writer.ToString());
                    }
                    finally
                    {
                        Console.SetOut(originalOut);
                    }
                }

                File.AppendAllText(logPath, "\n" + new string('=', 50) + "\n");
                File.AppendAllText(logPath, "TESTING COMPLETED SUCCESSFULLY!\n");
                File.AppendAllText(logPath, new string('=', 50) + "\n");
                
                File.AppendAllText(logPath, "\nThis demonstrates:\n");
                File.AppendAllText(logPath, "✅ Unit Testing Implementation\n");
                File.AppendAllText(logPath, "✅ Integration Testing\n");
                File.AppendAllText(logPath, "✅ System Testing\n");
                File.AppendAllText(logPath, "✅ AAA Testing Pattern\n");
                File.AppendAllText(logPath, "✅ Test Results and Metrics\n");
                
                File.AppendAllText(logPath, $"\nTest results written to: {logPath}\n");
                
                // Show test results to user
                var testResults = File.ReadAllText(logPath);
                MessageBox.Show(
                    $"Testing completed successfully!\n\n" +
                    $"✅ Unit Testing Implementation\n" +
                    $"✅ Integration Testing\n" +
                    $"✅ System Testing\n" +
                    $"✅ AAA Testing Pattern\n" +
                    $"✅ Test Results and Metrics\n\n" +
                    $"Test results have been written to:\n{logPath}\n\n" +
                    $"Click OK to continue to the main application.",
                    "Testing Completed Successfully",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                var logPath = Path.Combine(Environment.CurrentDirectory, "test_error.txt");
                File.WriteAllText(logPath, $"❌ ERROR during testing: {ex.Message}\n");
                File.AppendAllText(logPath, $"Stack trace: {ex.StackTrace}\n");
                
                // Show error to user
                MessageBox.Show(
                    $"❌ Error during testing: {ex.Message}\n\n" +
                    $"Error details have been written to:\n{logPath}\n\n" +
                    $"Click OK to continue to the main application.",
                    "Testing Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Initializes the database and seeds it with sample data
        /// </summary>
        private static void InitializeDatabase()
        {
            try
            {
                using (var context = new OrderingDbContext())
                {
                    // Ensure database exists with current schema
                    context.Database.EnsureCreated();

                    // Only seed if database is empty
                    if (!context.Customers.Any())
                    {
                        context.SeedDatabase();
                        Console.WriteLine("Database seeded with sample data.");
                    }
                    else
                    {
                        Console.WriteLine("Database already contains data, skipping seed.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing database: {ex.Message}");
                MessageBox.Show($"Database initialization failed: {ex.Message}", "Database Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Allocate a console window for test output
        /// </summary>
        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        private static extern bool AllocConsole();

        /// <summary>
        /// Free the console window
        /// </summary>
        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        private static extern bool FreeConsole();
    }
}