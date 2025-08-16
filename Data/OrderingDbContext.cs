using System;
using System.Collections.Generic;
using OnlineOrderingSystem.Models;
using System.Linq; // Added for .Where() and .ToList()
using Microsoft.EntityFrameworkCore;

namespace OnlineOrderingSystem.Data
{
    /// <summary>
    /// Entity Framework DbContext for the Online Ordering System
    /// </summary>
    public class OrderingDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentEntity> PaymentEntities { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderStatusUpdate> OrderStatusUpdates { get; set; }
        public DbSet<CustomizationOption> CustomizationOptions { get; set; }

        // Parameterless constructor for EF Core
        public OrderingDbContext()
        {
        }

        // Constructor for dependency injection
        public OrderingDbContext(DbContextOptions<OrderingDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Use SQL Server database
                optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=FoodOrderingSystem;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Customer entity
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CustomerID);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.Property(e => e.Address).HasMaxLength(250);
                entity.Property(e => e.Postcode).HasMaxLength(20);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PreferredPaymentMethod).HasMaxLength(50);
                
                // Configure navigation property
                entity.HasMany(c => c.OrderHistory)
                      .WithOne()
                      .HasForeignKey(o => o.CustomerID)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Item entity
            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Category).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Price).HasColumnType("decimal(10,2)");
                entity.Property(e => e.ImageURL).HasMaxLength(250);
                entity.Property(e => e.AllergenInfo).HasMaxLength(250);
                
                // Configure lists to be ignored by EF Core (not stored in database)
                entity.Ignore(e => e.DietaryTags);
                entity.Ignore(e => e.Ingredients);
            });

            // Configure Order entity
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderID);
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(10,2)");
                entity.Property(e => e.DeliveryFee).HasColumnType("decimal(10,2)");
                entity.Property(e => e.TaxAmount).HasColumnType("decimal(10,2)");
                entity.Property(e => e.Subtotal).HasColumnType("decimal(10,2)");
                entity.Property(e => e.OrderStatus).HasMaxLength(50);
                entity.Property(e => e.PaymentStatus).HasMaxLength(50);
                entity.Property(e => e.PaymentMethod).HasMaxLength(50);
                entity.Property(e => e.DeliveryAddress).HasMaxLength(250);
                entity.Property(e => e.DeliveryInstructions).HasMaxLength(500);
                entity.Property(e => e.SpecialRequests).HasMaxLength(500);
                entity.Property(e => e.OrderNotes).HasMaxLength(500);

                // Configure relationships
                entity.HasMany(o => o.OrderItems)
                      .WithOne()
                      .HasForeignKey(od => od.OrderID)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                entity.HasMany(o => o.StatusHistory)
                      .WithOne()
                      .HasForeignKey("OrderID")
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure OrderDetail entity
            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => e.OrderDetailID);
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.Price).HasColumnType("decimal(10,2)");
                entity.Property(e => e.TotalPrice).HasColumnType("decimal(10,2)");
                entity.Property(e => e.CustomizationCost).HasColumnType("decimal(10,2)");
                entity.Property(e => e.SpecialInstructions).HasMaxLength(500);
                entity.Property(e => e.DietaryNotes).HasMaxLength(250);
                entity.Property(e => e.AllergenInfo).HasMaxLength(250);
                
                // Configure Customizations to be ignored by EF Core (not stored in database)
                entity.Ignore(e => e.Customizations);
            });

            // Configure OrderStatusUpdate entity
            modelBuilder.Entity<OrderStatusUpdate>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Status).HasMaxLength(50);
                entity.Property(e => e.Message).HasMaxLength(500);
                entity.Property(e => e.Timestamp).IsRequired();
            });

            // Configure PaymentEntity for database storage
            modelBuilder.Entity<PaymentEntity>(entity =>
            {
                entity.HasKey(e => e.PaymentID);
                entity.Property(e => e.Amount).HasColumnType("decimal(10,2)");
                entity.Property(e => e.PaymentMethod).HasMaxLength(50);
                entity.Property(e => e.PaymentStatus).HasMaxLength(50);
                entity.Property(e => e.PaymentDetails).HasMaxLength(500);
                entity.Property(e => e.TransactionID).HasMaxLength(100);
                entity.Property(e => e.CardNumber).HasMaxLength(20);
                entity.Property(e => e.CardHolderName).HasMaxLength(100);
                entity.Property(e => e.ChequeNumber).HasMaxLength(50);
                entity.Property(e => e.BankName).HasMaxLength(100);
                entity.Property(e => e.AmountTendered).HasColumnType("decimal(10,2)");
                
                // Configure relationships
                entity.HasOne<Order>()
                      .WithMany()
                      .HasForeignKey(p => p.OrderID)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                entity.HasOne<Customer>()
                      .WithMany()
                      .HasForeignKey(p => p.CustomerID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure CustomizationOption entity to ignore List<string> properties
            modelBuilder.Entity<CustomizationOption>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.AdditionalCost).HasColumnType("decimal(10,2)");
                entity.Property(e => e.MaxSelections).IsRequired();
                
                // Ignore the List<string> Choices property to prevent database mapping issues
                entity.Ignore(e => e.Choices);
            });

            // Configure derived payment types
            modelBuilder.Entity<Cash>(entity =>
            {
                entity.Property(e => e.AmountTendered).HasColumnType("decimal(10,2)");
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(10,2)");
                entity.Property(e => e.ChangeDue).HasColumnType("decimal(10,2)");
            });

            modelBuilder.Entity<Credit>(entity =>
            {
                entity.Property(e => e.CardNumber).HasMaxLength(20);
                entity.Property(e => e.CardHolderName).HasMaxLength(100);
            });

            modelBuilder.Entity<Check>(entity =>
            {
                entity.Property(e => e.ChequeNumber).HasMaxLength(20);
                entity.Property(e => e.BankName).HasMaxLength(100);
            });
        }

        /// <summary>
        /// Seeds the database with sample data if it's empty
        /// </summary>
        public void SeedDatabase()
        {
            if (!Items.Any())
            {
                // Initialize comprehensive sample items for Tasty Eats
                var sampleItems = new[]
                {
                    // Starters
                    new Item { Name = "Garlic Bread", Description = "Fresh baked garlic bread with herbs", Price = 4.99, Category = "Starters", Available = true, PrepTime = 10, Rating = 4.5, Calories = 180, ReviewCount = 90, IsPopular = true, IsChefSpecial = false, AllergenInfo = "Contains: Gluten, Dairy" },
                    new Item { Name = "Bruschetta", Description = "Toasted bread with tomatoes and basil", Price = 5.99, Category = "Starters", Available = true, PrepTime = 8, Rating = 4.3, Calories = 120, ReviewCount = 86, IsPopular = false, IsChefSpecial = false, AllergenInfo = "Contains: Gluten" },
                    new Item { Name = "Chicken Wings", Description = "Crispy wings with your choice of sauce", Price = 8.99, Category = "Starters", Available = true, PrepTime = 15, Rating = 4.7, Calories = 280, ReviewCount = 94, IsPopular = true, IsChefSpecial = true, AllergenInfo = "May contain: Celery" },
                    new Item { Name = "Mozzarella Sticks", Description = "Golden fried mozzarella with marinara", Price = 6.99, Category = "Starters", Available = true, PrepTime = 12, Rating = 4.4, Calories = 240, ReviewCount = 88, IsPopular = false, IsChefSpecial = false, AllergenInfo = "Contains: Dairy, Gluten" },

                    // Main Courses
                    new Item { Name = "Margherita Pizza", Description = "Classic tomato and mozzarella", Price = 12.99, Category = "Main Courses", Available = true, PrepTime = 20, Rating = 4.6, Calories = 320, ReviewCount = 92, IsPopular = true, IsChefSpecial = false, AllergenInfo = "Contains: Gluten, Dairy" },
                    new Item { Name = "Pepperoni Pizza", Description = "Spicy pepperoni with cheese", Price = 14.99, Category = "Main Courses", Available = true, PrepTime = 20, Rating = 4.8, Calories = 380, ReviewCount = 96, IsPopular = true, IsChefSpecial = true, AllergenInfo = "Contains: Gluten, Dairy" },
                    new Item { Name = "Chicken Caesar Salad", Description = "Fresh lettuce with grilled chicken", Price = 11.99, Category = "Main Courses", Available = true, PrepTime = 12, Rating = 4.4, Calories = 290, ReviewCount = 88, IsPopular = false, IsChefSpecial = false, AllergenInfo = "Contains: Dairy, Eggs" },
                    new Item { Name = "Beef Burger", Description = "Juicy beef burger with fries", Price = 13.99, Category = "Main Courses", Available = true, PrepTime = 18, Rating = 4.7, Calories = 650, ReviewCount = 94, IsPopular = true, IsChefSpecial = true, AllergenInfo = "Contains: Gluten, Dairy" },
                    new Item { Name = "Fish & Chips", Description = "Crispy battered cod with chips", Price = 15.99, Category = "Main Courses", Available = true, PrepTime = 25, Rating = 4.5, Calories = 580, ReviewCount = 90, IsPopular = true, IsChefSpecial = false, AllergenInfo = "Contains: Fish, Gluten" },
                    new Item {  Name = "Grilled Salmon", Description = "Atlantic salmon with lemon butter", Price = 18.99, Category = "Main Courses", Available = true, PrepTime = 20, Rating = 4.9, Calories = 420, ReviewCount = 98, IsPopular = true, IsChefSpecial = true, AllergenInfo = "Contains: Fish, Dairy" },

                    // Desserts
                    new Item {  Name = "Chocolate Cake", Description = "Rich chocolate cake with cream", Price = 6.99, Category = "Desserts", Available = true, PrepTime = 10, Rating = 4.6, Calories = 450, ReviewCount = 92, IsPopular = true, IsChefSpecial = false, AllergenInfo = "Contains: Eggs, Dairy, Gluten" },
                    new Item {  Name = "Ice Cream", Description = "Vanilla ice cream with toppings", Price = 4.99, Category = "Desserts", Available = true, PrepTime = 5, Rating = 4.3, Calories = 220, ReviewCount = 86, IsPopular = false, IsChefSpecial = false, AllergenInfo = "Contains: Dairy, Eggs" },
                    new Item {  Name = "Tiramisu", Description = "Classic Italian dessert with coffee", Price = 7.99, Category = "Desserts", Available = true, PrepTime = 8, Rating = 4.8, Calories = 380, ReviewCount = 96, IsPopular = true, IsChefSpecial = true, AllergenInfo = "Contains: Eggs, Dairy, Gluten" },

                    // Drinks
                    new Item {  Name = "Soft Drinks", Description = "Coca-Cola, Sprite, Fanta", Price = 2.99, Category = "Drinks", Available = true, PrepTime = 2, Rating = 4.2, Calories = 150, ReviewCount = 84, IsPopular = false, IsChefSpecial = false, AllergenInfo = "None" },
                    new Item {  Name = "Coffee", Description = "Fresh brewed coffee", Price = 3.50, Category = "Drinks", Available = true, PrepTime = 3, Rating = 4.4, Calories = 5, ReviewCount = 88, IsPopular = false, IsChefSpecial = false, AllergenInfo = "May contain: Dairy" },
                    new Item {  Name = "Tea", Description = "English breakfast tea", Price = 2.50, Category = "Drinks", Available = true, PrepTime = 2, Rating = 4.1, Calories = 2, ReviewCount = 82, IsPopular = false, IsChefSpecial = false, AllergenInfo = "None" },
                    new Item {  Name = "Fresh Orange Juice", Description = "Freshly squeezed orange juice", Price = 4.50, Category = "Drinks", Available = true, PrepTime = 3, Rating = 4.6, Calories = 110, ReviewCount = 92, IsPopular = true, IsChefSpecial = false, AllergenInfo = "None" }
                };

                // Add dietary tags and ingredients for each item
                AddDietaryTagsAndIngredients(sampleItems);

                Items.AddRange(sampleItems);
            }

            if (!Customers.Any())
            {
                // Initialize sample customers
                var sampleCustomers = new[]
                {
                    new Customer { FirstName = "John", LastName = "Doe", Email = "john@example.com", Address = "123 Main St, London, UK", PreferredPaymentMethod = "Credit" },
                    new Customer { FirstName = "Jane", LastName = "Smith", Email = "jane@example.com", Address = "456 Oak Ave, Manchester, UK", PreferredPaymentMethod = "Cash" },
                    new Customer { FirstName = "Mike", LastName = "Johnson", Email = "mike@example.com", Address = "789 Pine Rd, Birmingham, UK", PreferredPaymentMethod = "Credit" }
                };

                Customers.AddRange(sampleCustomers);
            }

            // Note: Carts are not persisted to database - they are created in memory when needed
            // Sample carts will be created when customers place orders

            SaveChanges();
        }

        /// <summary>
        /// Adds dietary tags and ingredients to sample items
        /// </summary>
        /// <param name="items">Array of items to configure</param>
        private void AddDietaryTagsAndIngredients(Item[] items)
        {
            // Garlic Bread
            items[0].DietaryTags.AddRange(new[] { "Vegetarian" });
            items[0].Ingredients.AddRange(new[] { "Garlic", "Butter", "Bread", "Herbs", "Parsley" });

            // Bruschetta
            items[1].DietaryTags.AddRange(new[] { "Vegetarian", "Vegan Option" });
            items[1].Ingredients.AddRange(new[] { "Tomatoes", "Basil", "Garlic", "Olive Oil", "Bread" });

            // Chicken Wings
            items[2].DietaryTags.AddRange(new[] { "Spicy", "Gluten-Free" });
            items[2].Ingredients.AddRange(new[] { "Chicken Wings", "Buffalo Sauce", "Celery", "Blue Cheese" });

            // Mozzarella Sticks
            items[3].DietaryTags.AddRange(new[] { "Vegetarian" });
            items[3].Ingredients.AddRange(new[] { "Mozzarella", "Breadcrumbs", "Marinara Sauce", "Flour" });

            // Margherita Pizza
            items[4].DietaryTags.AddRange(new[] { "Vegetarian" });
            items[4].Ingredients.AddRange(new[] { "Pizza Dough", "Tomato Sauce", "Mozzarella", "Fresh Basil" });

            // Pepperoni Pizza
            items[5].DietaryTags.AddRange(new[] { "Spicy" });
            items[5].Ingredients.AddRange(new[] { "Pizza Dough", "Tomato Sauce", "Mozzarella", "Pepperoni" });

            // Chicken Caesar Salad
            items[6].DietaryTags.AddRange(new[] { "High Protein", "Low Carb" });
            items[6].Ingredients.AddRange(new[] { "Romaine Lettuce", "Grilled Chicken", "Caesar Dressing", "Parmesan", "Croutons" });

            // Beef Burger
            items[7].DietaryTags.AddRange(new[] { "Popular", "High Protein" });
            items[7].Ingredients.AddRange(new[] { "Beef Patty", "Burger Bun", "Lettuce", "Tomato", "Cheese", "Fries" });

            // Fish & Chips
            items[8].DietaryTags.AddRange(new[] { "British Classic", "High Protein" });
            items[8].Ingredients.AddRange(new[] { "Cod", "Batter", "Chips", "Mushy Peas", "Tartar Sauce" });

            // Grilled Salmon
            items[9].DietaryTags.AddRange(new[] { "Healthy", "High Protein", "Omega-3", "Gluten-Free" });
            items[9].Ingredients.AddRange(new[] { "Atlantic Salmon", "Lemon", "Butter", "Herbs", "Vegetables" });

            // Chocolate Cake
            items[10].DietaryTags.AddRange(new[] { "Vegetarian", "Sweet" });
            items[10].Ingredients.AddRange(new[] { "Chocolate", "Flour", "Sugar", "Eggs", "Butter", "Cream" });

            // Ice Cream
            items[11].DietaryTags.AddRange(new[] { "Vegetarian", "Sweet", "Cold" });
            items[11].Ingredients.AddRange(new[] { "Cream", "Sugar", "Vanilla", "Milk" });

            // Tiramisu
            items[12].DietaryTags.AddRange(new[] { "Vegetarian", "Italian", "Coffee Flavored" });
            items[12].Ingredients.AddRange(new[] { "Mascarpone", "Coffee", "Ladyfingers", "Cocoa", "Sugar" });

            // Soft Drinks
            items[13].DietaryTags.AddRange(new[] { "Vegetarian", "Vegan", "Refreshing" });
            items[13].Ingredients.AddRange(new[] { "Carbonated Water", "Natural Flavors", "Sugar" });

            // Coffee
            items[14].DietaryTags.AddRange(new[] { "Vegetarian", "Vegan Option", "Caffeinated" });
            items[14].Ingredients.AddRange(new[] { "Coffee Beans", "Water" });

            // Tea
            items[15].DietaryTags.AddRange(new[] { "Vegetarian", "Vegan", "Caffeinated" });
            items[15].Ingredients.AddRange(new[] { "Tea Leaves", "Water" });

            // Fresh Orange Juice
            items[16].DietaryTags.AddRange(new[] { "Vegetarian", "Vegan", "Fresh", "Vitamin C" });
            items[16].Ingredients.AddRange(new[] { "Fresh Oranges" });
        }

        /// <summary>
        /// Gets all items
        /// </summary>
        /// <returns>List of all items</returns>
        public List<Item> GetAllItems()
        {
            return Items.ToList();
        }

        /// <summary>
        /// Gets items by category
        /// </summary>
        /// <param name="category">The category to filter by</param>
        /// <returns>List of items in the specified category</returns>
        public List<Item> GetItemsByCategory(string category)
        {
            return Items.Where(item => item.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        /// <summary>
        /// Gets a customer by ID
        /// </summary>
        /// <param name="customerID">The customer ID</param>
        /// <returns>The customer or null if not found</returns>
        public Customer? GetCustomerById(int customerID)
        {
            return Customers.FirstOrDefault(c => c.CustomerID == customerID);
        }

        /// <summary>
        /// Adds a new order
        /// </summary>
        /// <param name="order">The order to add</param>
        public void AddOrder(Order order)
        {
            Orders.Add(order);
            SaveChanges();
        }

        /// <summary>
        /// Gets orders for a customer
        /// </summary>
        /// <param name="customerID">The customer ID</param>
        /// <returns>List of orders for the customer</returns>
        public List<Order> GetOrdersByCustomer(int customerID)
        {
            return Orders.Where(o => o.CustomerID == customerID).ToList();
        }
    }
} 