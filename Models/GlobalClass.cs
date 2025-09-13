using System;
using System.Collections.Generic;

namespace OnlineOrderingSystem.Models
{
    /// <summary>
    /// Singleton class for global system configuration and utilities.
    /// 
    /// Demonstrates:
    /// - Singleton Pattern for global access
    /// - Configuration management
    /// - Logging and database connection management
    /// - Thread-safe singleton implementation
    /// 
    /// Note: This is demonstration code - not actively used in the current application.
    /// </summary>
    public class GlobalClass
    {
        private static GlobalClass? _instance; // Singleton instance
        private static readonly object _lock = new object(); // Lock object for thread safety

        public Dictionary<string, string> SystemConfig { get; set; } = new Dictionary<string, string>(); // Configuration settings
        public Logger Logger { get; set; } = new Logger(); // Logger instance
        public DBConnection DatabaseConnection { get; set; } = new DBConnection(); // Database connection instance

        /// <summary>
        /// Private constructor to prevent external instantiation
        /// </summary>
        private GlobalClass()
        {
            LoadConfiguration();
        }

        /// <summary>
        /// Gets the singleton instance of GlobalClass
        /// </summary>
        /// <returns>The singleton instance</returns>
        public static GlobalClass GetInstance() // Thread-safe singleton access
        {
            if (_instance == null) // First check
            {
                lock (_lock) // Ensure thread safety
                {
                    if (_instance == null) // Second check
                    {
                        _instance = new GlobalClass(); // Create instance
                    }
                }
            }
            return _instance;
        }

        /// <summary>
        /// Logs an event with the specified type and message
        /// </summary>
        /// <param name="eventType">The type of event</param>
        /// <param name="message">The message to log</param>
        public void LogEvent(string eventType, string message)
        {
            Logger.Log($"[{eventType}] {message}");
        }

        /// <summary>
        /// Gets the database connection
        /// </summary>
        /// <returns>The database connection</returns>
        public DBConnection GetConnection()
        {
            return DatabaseConnection;
        }

        /// <summary>
        /// Loads the system configuration
        /// </summary>
        public void LoadConfiguration()
        {
            // Load default configuration
            SystemConfig["DatabaseConnectionString"] = "Server=localhost;Database=OnlineOrdering;Trusted_Connection=true;TrustServerCertificate=true;";
            SystemConfig["LogLevel"] = "Info";
            SystemConfig["ApplicationName"] = "Online Ordering System";
            SystemConfig["Version"] = "1.0.0";
        }

        /// <summary>
        /// Sends a notification to the specified recipient
        /// </summary>
        /// <param name="message">The message to send</param>
        /// <param name="recipient">The recipient</param>
        public void SendNotification(string message, string recipient)
        {
            LogEvent("Notification", $"Sending notification to {recipient}: {message}");
            Console.WriteLine($"Notification sent to {recipient}: {message}");
        }
    }

    /// <summary>
    /// Simple logger class for demonstration
    /// </summary>
    public class Logger
    {
        public void Log(string message)
        {
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}");
        }
    }

    /// <summary>
    /// Database connection class for demonstration
    /// </summary>
    public class DBConnection
    {
        public string ConnectionString { get; set; } = string.Empty;
        public bool IsConnected { get; set; }

        public void Connect()
        {
            IsConnected = true;
            Console.WriteLine("Database connected successfully.");
        }

        public void Disconnect()
        {
            IsConnected = false;
            Console.WriteLine("Database disconnected.");
        }
    }

    /// <summary>
    /// Global user session management
    /// </summary>
    public static class GlobalUser
    {
        public static object? CurrentUser { get; set; }
        public static Customer? CurrentCustomer { get; set; }

        public static void Logout()
        {
            CurrentUser = null;
            CurrentCustomer = null;
        }

        public static bool IsLoggedIn => CurrentCustomer != null || CurrentUser != null;
    }
} 