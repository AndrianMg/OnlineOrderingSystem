using System;
using OnlineOrderingSystem.Database;
using OnlineOrderingSystem.Data;

namespace OnlineOrderingSystem
{
    /// <summary>
    /// Simple test class to verify database contains menu items
    /// </summary>
    public class TestMenuDatabase
    {
        public static void TestDatabaseItems()
        {
            Console.WriteLine("=== Testing Menu Database ===");
            
            try
            {
                // Test direct database access
                using (var context = new OrderingDbContext())
                {
                    Console.WriteLine($"Items count in database: {context.Items.Count()}");
                    
                    // Seed if empty
                    if (!context.Items.Any())
                    {
                        Console.WriteLine("Database is empty, seeding data...");
                        context.SeedDatabase();
                        Console.WriteLine($"After seeding: {context.Items.Count()} items");
                    }
                    
                    // Show first few items
                    var items = context.Items.Take(5).ToList();
                    Console.WriteLine("\nFirst 5 items in database:");
                    foreach (var item in items)
                    {
                        Console.WriteLine($"- {item.Name} (£{item.Price:F2}) - {item.Category}");
                        Console.WriteLine($"  Rating: {item.Rating:F1}★ | {item.PrepTime}min | {item.Calories}cal");
                        if (item.DietaryTags.Any())
                        {
                            Console.WriteLine($"  Tags: {string.Join(", ", item.DietaryTags)}");
                        }
                        Console.WriteLine();
                    }
                }
                
                // Test using MenuDataAccess
                Console.WriteLine("=== Testing MenuDataAccess ===");
                var menuAccess = new MenuDataAccess();
                var allItems = menuAccess.GetAllMenuItems();
                Console.WriteLine($"MenuDataAccess returned {allItems.Count} items");
                
                var availableItems = menuAccess.GetAvailableMenuItems();
                Console.WriteLine($"Available items: {availableItems.Count}");
                
                var categories = allItems.Select(i => i.Category).Distinct().ToList();
                Console.WriteLine($"Categories: {string.Join(", ", categories)}");
                
                Console.WriteLine("\n=== Test Completed Successfully ===");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error testing database: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }
    }
}
