using OnlineOrderingSystem.Forms;
using System;

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
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            
            // Start with LoginForm as intended
            Application.Run(new LoginForm());
        }
    }
}