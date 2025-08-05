# 🍕 Tasty Eats - Online Ordering System

A comprehensive Windows Forms application for restaurant food ordering with MySQL database integration.

## 📋 Features

### 🎨 **Enhanced User Interface**
- **Modern Dark Theme**: Professional dark interface with consistent styling
- **Responsive Design**: Optimized for different screen sizes
- **Interactive Elements**: Hover effects, animations, and visual feedback
- **Accessibility**: Keyboard shortcuts and tooltips

### 🍽️ **Menu Management**
- **Category Tabs**: Easy navigation between Starters, Mains, Desserts, Drinks
- **Search Functionality**: Find items by name, description, or dietary tags
- **Dietary Information**: Visual tags for Vegetarian, Spicy, Healthy, Gluten-Free options
- **Star Ratings**: Customer reviews and ratings display
- **Preparation Times**: Estimated cooking times for each item

### 🛒 **Shopping Cart**
- **Live Cart Preview**: Real-time updates as items are added
- **Quantity Management**: Easy quantity adjustment
- **Item Removal**: Remove items directly from cart
- **Total Calculation**: Automatic price calculations

### 📋 **Order History**
- **Comprehensive Tracking**: View all past orders with details
- **Status Filtering**: Filter by Delivered, In Progress, Preparing
- **Export Options**: PDF export and print functionality
- **Progress Tracking**: Visual progress bars for active orders
- **Loyalty Rewards**: Track savings and rewards

### 🔧 **Database Integration**
- **MySQL Database**: Full database integration with sample data
- **Data Persistence**: Orders, users, and menu items stored in database
- **Connection Management**: Easy database setup and configuration
- **Fallback System**: Graceful handling when database is unavailable

## 🚀 Quick Start

### Prerequisites
- **.NET 6.0** or later
- **MySQL Server** (8.0 or later recommended)
- **MySQL Workbench** (optional, for database management)

### Installation Steps

#### 1. **Clone/Download the Project**
```bash
git clone <repository-url>
cd OnlineOrderingSystem
```

#### 2. **Set Up MySQL Database**

**Option A: Using MySQL Workbench**
1. Open MySQL Workbench
2. Connect to your MySQL server
3. Open the SQL script: `Database/tasty_eats_schema.sql`
4. Execute the script to create the database and sample data

**Option B: Using Command Line**
```bash
mysql -u root -p < Database/tasty_eats_schema.sql
```

#### 3. **Configure Database Connection**
1. Run the application: `dotnet run`
2. Click the "🔧 Database" button on the main form
3. Enter your MySQL connection details:
   - **Server**: `localhost` (or your MySQL server address)
   - **Database**: `tasty_eats_db`
   - **Username**: `root` (or your MySQL username)
   - **Password**: Your MySQL password
   - **Port**: `3306` (default MySQL port)
4. Click "🔍 Test Connection" to verify
5. Click "💾 Save Settings" to save the configuration

#### 4. **Run the Application**
```bash
dotnet run
```

## 🗄️ Database Schema

### **Core Tables**
- **`users`**: Customer accounts and profiles
- **`menu_items`**: Food items with prices, descriptions, and categories
- **`categories`**: Menu categories (Starters, Mains, Desserts, Drinks)
- **`dietary_tags`**: Dietary information (Vegetarian, Spicy, etc.)
- **`orders`**: Order records with status and delivery information
- **`order_items`**: Individual items within each order
- **`cart_items`**: Temporary shopping cart storage

### **Sample Data Included**
- **13 Menu Items**: Complete menu with realistic items and prices
- **4 Categories**: Starters, Mains, Desserts, Drinks
- **7 Dietary Tags**: Vegetarian, Spicy, Healthy, Gluten-Free, Popular, Chef's Pick, British Classic
- **Sample Orders**: 4 example orders with different statuses
- **Test Users**: 
  - **John Doe**: `john.doe@example.com` / `123456`
  - **Test User**: `test@example.com` / `123456`

## 🎮 How to Use

### **Main Application Flow**
1. **Login/Register**: Create an account or log in
2. **Browse Menu**: Use category tabs or search to find items
3. **Add to Cart**: Click on items to add them to your cart
4. **Review Cart**: Check your cart contents and adjust quantities
5. **Checkout**: Complete your order with delivery details
6. **Track Orders**: View order history and track delivery status

### **Keyboard Shortcuts**
- **H**: Open Order History
- **M**: Open Menu
- **P**: Open Profile
- **Esc**: Exit Application

### **Database Features**
- **Automatic Fallback**: If database is unavailable, app uses sample data
- **Connection Testing**: Built-in connection testing in setup form
- **Error Handling**: Graceful error messages for database issues
- **Data Persistence**: All orders and user data saved to database

### **Authentication System**
- **Secure Password Hashing**: Passwords are hashed using SHA256
- **User Registration**: New users can register with email validation
- **User Login**: Database-driven authentication with error handling
- **Session Management**: Global user session tracking
- **Test Accounts**: Pre-configured test users for immediate testing

## 🔧 Technical Details

### **Architecture**
- **Frontend**: Windows Forms (.NET 6.0)
- **Backend**: C# with MySQL database
- **Data Access**: Custom data access layer with connection pooling
- **UI Framework**: Native Windows Forms with custom styling

### **Key Components**
- **`DatabaseConfig.cs`**: Centralized database connection management
- **`MenuDataAccess.cs`**: Menu item database operations
- **`OrderDataAccess.cs`**: Order and cart database operations
- **`UserDataAccess.cs`**: User authentication and registration operations
- **`DatabaseSetupForm.cs`**: Database configuration interface

### **Database Connection**
```csharp
// Default connection string
Server=localhost;Database=tasty_eats_db;Uid=root;Pwd=;Port=3306;
```

## 🐛 Troubleshooting

### **Common Issues**

#### **Database Connection Failed**
- Verify MySQL server is running
- Check username/password in database setup
- Ensure database `tasty_eats_db` exists
- Verify port 3306 is accessible

#### **Build Errors**
- Ensure .NET 6.0 SDK is installed
- Run `dotnet restore` to restore packages
- Check MySQL.Data package is properly referenced

#### **Application Won't Start**
- Check all required files are present
- Verify .NET runtime is installed
- Run `dotnet build` to check for compilation errors

### **Sample Data Fallback**
If the database is not available, the application will:
1. Show a warning message
2. Load sample menu items from memory
3. Continue functioning with local data
4. Display database setup option

## 📁 Project Structure

```
OnlineOrderingSystem/
├── Database/
│   ├── tasty_eats_schema.sql    # Database schema and sample data
│   ├── DatabaseConfig.cs        # Database connection management
│   ├── MenuDataAccess.cs        # Menu item database operations
│   └── OrderDataAccess.cs       # Order and cart database operations
├── Forms/
│   ├── MainForm.cs              # Main application window
│   ├── LoginForm.cs             # User authentication
│   ├── EnhancedMenuForm.cs      # Menu browsing interface
│   ├── OrderHistoryForm.cs      # Order history and tracking
│   ├── DatabaseSetupForm.cs     # Database configuration
│   └── ...                      # Other form files
├── Models/
│   ├── Item.cs                  # Menu item model
│   └── Cart.cs                  # Shopping cart model
└── Program.cs                   # Application entry point
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly
5. Submit a pull request

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🆘 Support

If you encounter any issues:
1. Check the troubleshooting section above
2. Verify your MySQL installation and configuration
3. Ensure all prerequisites are installed
4. Check the application logs for detailed error messages

---

**Enjoy your Tasty Eats experience! 🍕🍔🍰** 