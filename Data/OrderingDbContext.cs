using System;
using System.Collections.Generic;
using OnlineOrderingSystem.Models;
using System.Linq; // Added for .Where() and .ToList()
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using OnlineOrderingSystem.Data.Seeders;

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
                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["OrderingDbContext"].ConnectionString);
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
                
                // Map CustomizationsJson to the Customizations database column
                entity.Property(e => e.CustomizationsJson).HasColumnName("Customizations");
                
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
            ItemSeeder.Seed(this);
            CustomerSeeder.Seed(this);
            SaveChanges();
        }

        
    }
} 