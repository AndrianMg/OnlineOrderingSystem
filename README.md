# 🍽️ Tasty Eats - Online Ordering System

A modern, feature-rich Windows Forms application for a restaurant online ordering system, built with C# and .NET. This project utilizes Entity Framework Core for data persistence and showcases several key software design patterns.

![License](https://img.shields.io/badge/license-MIT-blue.svg)
![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)
![Platform](https://img.shields.io/badge/platform-Windows-lightgrey.svg)

## 📋 Features

- **Modern UI:** A clean, themed interface for a pleasant user experience.
- **Menu System:** Browse items by category, view details, and search for specific items.
- **Item Customization:** Add or remove optional ingredients for menu items.
- **Dynamic Shopping Cart:** Add items to a cart, adjust quantities, and see the total price update in real-time.
- **Order History:** Registered users can view their past orders and check details.
- **Centralized Exception Handling:** A dedicated (though currently demonstrative) service for handling application-wide exceptions.
- **Database Integration:** Uses Entity Framework Core to manage the database schema and data.

## 🔧 Technology Stack

- **Backend & Frontend:** C# with .NET 8 and Windows Forms.
- **Database:** Microsoft SQL Server.
- **ORM:** Entity Framework Core for data access and migrations.

## 🚀 Quick Start Guide

### Prerequisites

- **.NET 8 SDK**
- **Microsoft SQL Server:** An accessible instance (like SQL Server Express) is required.

### 1. Configure the Database Connection

The connection string for the database is defined within the `OrderingDbContext.cs` file. You must update it to point to your SQL Server instance.

1.  Open the file: `OnlineOrderingSystem/Data/OrderingDbContext.cs`.
2.  Locate the `OnConfiguring` method.
3.  Modify the connection string in the `optionsBuilder.UseSqlServer()` call. For example:

    ```csharp
    // Example for SQL Server Express
    optionsBuilder.UseSqlServer("Server=.\SQLEXPRESS;Database=OnlineOrderingSystem;Trusted_Connection=True;TrustServerCertificate=true;");
    ```

### 2. Create the Database

This project uses EF Core migrations to set up the database.

1.  Open a terminal or command prompt in the project's root directory (`c:\Users\deeja\OnlineOrderingSystem\OnlineOrderingSystem`).
2.  Run the following command to apply the migrations and create the database:

    ```bash
    dotnet ef database update
    ```

### 3. Run the Application

Once the database is set up, you can build and run the application.

```bash
dotnet run
```

## 🗄️ Project Structure

The project is organized into the following key directories:

```
OnlineOrderingSystem/
├── Data/
│   ├── OrderingDbContext.cs     # EF Core database context.
│   └── Seeders/                 # Classes to populate the database with initial data.
├── Forms/
│   ├── MainForm.cs              # Main application window.
│   ├── EnhancedMenuForm.cs      # The primary menu interface.
│   ├── CartForm.cs              # The user's shopping cart.
│   └── ...                      # Other UI forms.
├── Migrations/
│   └── ...                      # EF Core migration files.
├── Models/
│   ├── Item.cs                  # Data model for a menu item.
│   ├── Order.cs                 # Data model for a customer order.
│   └── ...                      # Other data models.
├── Services/
│   ├── OrderingService.cs       # Business logic for placing orders.
│   ├── ExceptionHandler.cs      # (Demonstration) Centralized error handling.
│   └── NotificationService.cs   # (Demonstration) Manages notifications.
└── Program.cs                   # The application entry point.
```

## 🧠 Design Patterns Used

This project demonstrates several important design patterns:

-   **Singleton:** The `GlobalClass` and `NotificationService` use the Singleton pattern to ensure only one instance of each exists.
-   **Observer:** The `NotificationService` implements the Observer pattern to notify different parts of the application about events.
-   **Strategy:** The `ExceptionHandler` uses a Strategy pattern (via a switch on the exception type) to apply different handling logic for different errors.

## 📄 License

This project is licensed under the MIT License.