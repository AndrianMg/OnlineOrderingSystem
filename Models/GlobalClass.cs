using System;
using System.Collections.Generic;

namespace OnlineOrderingSystem.Models
{
    /// <summary>
    /// Singleton class for global system configuration and utilities
    /// </summary>
    public class GlobalClass
    {
        private static GlobalClass? _instance;
        private static readonly object _lock = new object();

        public Dictionary<string, string> SystemConfig { get; set; } = new Dictionary<string, string>();
        public Logger Logger { get; set; } = new Logger();
        public DBConnection DatabaseConnection { get; set; } = new DBConnection();

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
        public static GlobalClass GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new GlobalClass();
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
            SystemConfig["DatabaseConnectionString"] = "Server=localhost;Database=OnlineOrdering;Trusted_Connection=true;";
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

        public static void Logout()
        {
            CurrentUser = null;
        }

        public static bool IsLoggedIn => CurrentUser != null;
    }
} 