using OnlineOrderingSystem.Models;
using System.Collections.Generic;
using System.Linq;

namespace OnlineOrderingSystem.Data.Seeders
{
    public static class ItemSeeder
    {
        public static void Seed(OrderingDbContext context)
        {
            if (!context.Items.Any())
            {
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

                AddDietaryTagsAndIngredients(sampleItems);

                context.Items.AddRange(sampleItems);
            }
        }

        private static void AddDietaryTagsAndIngredients(Item[] items)
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
    }
}