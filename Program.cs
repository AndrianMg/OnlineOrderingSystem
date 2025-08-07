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
        /// <param name="args">Command line arguments (not used in this application)</param>
        [STAThread]
        static void Main(string[] args)
        {
            // Initialize the database and seed sample data
            InitializeDatabase();

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            
            // Start with LoginForm as intended
            Application.Run(new LoginForm());
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
                    // Ensure database is created
                    context.Database.EnsureCreated();
                    
                    // Seed sample data
                    context.SeedDatabase();
                    
                    Console.WriteLine("Database initialized successfully with sample data.");
                }

                // Run database functionality test (optional - can be commented out for production)
                // DatabaseDemo.RunDatabaseTest();
                // TestMenuDatabase.TestDatabaseItems();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing database: {ex.Message}");
                MessageBox.Show($"Database initialization failed: {ex.Message}", "Database Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}