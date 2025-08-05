using System;
using System.Collections.Generic;

namespace OnlineOrderingSystem.Models
{
    /// <summary>
    /// Represents a menu item in the restaurant ordering system
    /// Contains all information about a food item including pricing, availability, and dietary information
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Unique identifier for the menu item
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the menu item (e.g., "Margherita Pizza")
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Detailed description of the menu item
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Price of the item in pounds sterling
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Category the item belongs to (e.g., "Starters", "Mains", "Desserts", "Drinks")
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// Whether the item is currently available for ordering
        /// </summary>
        public bool Available { get; set; }

        /// <summary>
        /// Preparation time in minutes
        /// </summary>
        public int PrepTime { get; set; }

        /// <summary>
        /// Customer rating of the item (0.0 to 5.0)
        /// </summary>
        public double Rating { get; set; }

        /// <summary>
        /// List of dietary tags (e.g., "Vegetarian", "Gluten-Free", "Spicy")
        /// </summary>
        public List<string> DietaryTags { get; set; } = new List<string>();

        // Legacy properties for backward compatibility
        /// <summary>
        /// Legacy property for ItemID - provides backward compatibility
        /// </summary>
        public int ItemID { get => Id; set => Id = value; }

        /// <summary>
        /// Legacy property for Availability - provides backward compatibility
        /// </summary>
        public bool Availability { get => Available; set => Available = value; }

        /// <summary>
        /// Legacy property for PreparationTime - provides backward compatibility
        /// </summary>
        public int PreparationTime { get => PrepTime; set => PrepTime = value; }

        /// <summary>
        /// Alias property for Id - provides backward compatibility
        /// </summary>
        public int ItemId => Id;

        /// <summary>
        /// Alias property for PrepTime - provides backward compatibility
        /// </summary>
        public int PreparationTimeAlias => PrepTime;

        // Additional properties for enhanced functionality
        /// <summary>
        /// URL for the item's image
        /// </summary>
        public string ImageURL { get; set; } = string.Empty;

        /// <summary>
        /// List of ingredients in the item
        /// </summary>
        public List<string> Ingredients { get; set; } = new List<string>();

        /// <summary>
        /// List of customization options available for this item
        /// </summary>
        public List<CustomizationOption> CustomizationOptions { get; set; } = new List<CustomizationOption>();

        /// <summary>
        /// Number of reviews for this item
        /// </summary>
        public int ReviewCount { get; set; }

        /// <summary>
        /// Whether this item is marked as popular
        /// </summary>
        public bool IsPopular { get; set; }

        /// <summary>
        /// Whether this item is a chef's special
        /// </summary>
        public bool IsChefSpecial { get; set; }

        /// <summary>
        /// Allergen information for this item
        /// </summary>
        public string AllergenInfo { get; set; } = string.Empty;

        /// <summary>
        /// Calorie count for this item
        /// </summary>
        public int Calories { get; set; }

        /// <summary>
        /// Default constructor for creating new menu items
        /// </summary>
        public Item()
        {
            // Initialize with default values
            Id = 0;
            Name = string.Empty;
            Description = string.Empty;
            Price = 0.0;
            Category = string.Empty;
            Available = true;
            PrepTime = 0;
            Rating = 0.0;
            DietaryTags = new List<string>();
            Ingredients = new List<string>();
            CustomizationOptions = new List<CustomizationOption>();
            ReviewCount = 0;
            IsPopular = false;
            IsChefSpecial = false;
            AllergenInfo = string.Empty;
            Calories = 0;
        }

        /// <summary>
        /// Constructor for creating menu items with all properties
        /// </summary>
        /// <param name="id">Unique identifier</param>
        /// <param name="name">Item name</param>
        /// <param name="description">Item description</param>
        /// <param name="price">Item price</param>
        /// <param name="category">Item category</param>
        /// <param name="available">Whether item is available</param>
        /// <param name="prepTime">Preparation time in minutes</param>
        /// <param name="rating">Customer rating</param>
        /// <param name="dietaryTags">List of dietary tags</param>
        public Item(int id, string name, string description, double price, string category, bool available, int prepTime, double rating, List<string> dietaryTags)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            Category = category;
            Available = available;
            PrepTime = prepTime;
            Rating = rating;
            DietaryTags = dietaryTags ?? new List<string>();
            Ingredients = new List<string>();
            CustomizationOptions = new List<CustomizationOption>();
            ReviewCount = 0;
            IsPopular = false;
            IsChefSpecial = false;
            AllergenInfo = string.Empty;
            Calories = 0;
        }

        /// <summary>
        /// Creates a string representation of the menu item
        /// </summary>
        /// <returns>Formatted string with item details</returns>
        public override string ToString()
        {
            return $"{Name} - £{Price:F2} ({Category})";
        }

        /// <summary>
        /// Checks if the item has a specific dietary tag
        /// </summary>
        /// <param name="tag">Dietary tag to check for</param>
        /// <returns>True if the item has the specified tag</returns>
        public bool HasDietaryTag(string tag)
        {
            return DietaryTags.Contains(tag, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Adds a dietary tag to the item if it doesn't already exist
        /// </summary>
        /// <param name="tag">Dietary tag to add</param>
        public void AddDietaryTag(string tag)
        {
            if (!string.IsNullOrWhiteSpace(tag) && !HasDietaryTag(tag))
            {
                DietaryTags.Add(tag);
            }
        }

        /// <summary>
        /// Removes a dietary tag from the item
        /// </summary>
        /// <param name="tag">Dietary tag to remove</param>
        public void RemoveDietaryTag(string tag)
        {
            DietaryTags.RemoveAll(t => string.Equals(t, tag, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Calculates the total price including any applicable taxes or fees
        /// </summary>
        /// <param name="taxRate">Tax rate as a decimal (e.g., 0.20 for 20%)</param>
        /// <returns>Total price including tax</returns>
        public double GetTotalPrice(double taxRate = 0.0)
        {
            return Price * (1 + taxRate);
        }

        /// <summary>
        /// Checks if the item is suitable for a specific dietary requirement
        /// </summary>
        /// <param name="dietaryRequirement">Dietary requirement to check</param>
        /// <returns>True if the item meets the dietary requirement</returns>
        public bool IsSuitableFor(string dietaryRequirement)
        {
            return HasDietaryTag(dietaryRequirement);
        }

        /// <summary>
        /// Gets a formatted string of all dietary tags
        /// </summary>
        /// <returns>Comma-separated list of dietary tags</returns>
        public string GetDietaryTagsString()
        {
            return string.Join(", ", DietaryTags);
        }

        /// <summary>
        /// Validates that the item has all required properties set
        /// </summary>
        /// <returns>True if the item is valid</returns>
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name) && 
                   !string.IsNullOrWhiteSpace(Category) && 
                   Price >= 0 && 
                   PrepTime >= 0 && 
                   Rating >= 0 && 
                   Rating <= 5;
        }

        /// <summary>
        /// Gets the item details as a formatted string
        /// </summary>
        /// <returns>Formatted item details</returns>
        public string GetItemDetails()
        {
            return $"ID: {Id}, Name: {Name}, Price: £{Price:F2}, Category: {Category}, Available: {Available}, Rating: {Rating:F1}/5";
        }

        /// <summary>
        /// Updates the price of the item
        /// </summary>
        /// <param name="newPrice">The new price</param>
        public void UpdatePrice(double newPrice)
        {
            if (newPrice < 0)
            {
                throw new ArgumentException("Price cannot be negative");
            }

            Price = newPrice;
        }

        /// <summary>
        /// Checks if the item is available
        /// </summary>
        /// <returns>True if available, false otherwise</returns>
        public bool IsAvailable()
        {
            return Available;
        }

        /// <summary>
        /// Adds an ingredient to the item
        /// </summary>
        /// <param name="ingredient">The ingredient to add</param>
        public void AddIngredient(string ingredient)
        {
            if (!string.IsNullOrWhiteSpace(ingredient) && !Ingredients.Contains(ingredient))
            {
                Ingredients.Add(ingredient);
            }
        }

        /// <summary>
        /// Updates the description of the item
        /// </summary>
        /// <param name="newDescription">The new description</param>
        public void UpdateDescription(string newDescription)
        {
            Description = newDescription ?? string.Empty;
        }

        /// <summary>
        /// Gets a formatted list of ingredients
        /// </summary>
        /// <returns>Comma-separated list of ingredients</returns>
        public string GetIngredientsList()
        {
            return string.Join(", ", Ingredients);
        }

        /// <summary>
        /// Sets the availability status of the item
        /// </summary>
        /// <param name="available">Whether the item is available</param>
        public void SetAvailability(bool available)
        {
            Available = available;
        }

        /// <summary>
        /// Adds a customization option to the item
        /// </summary>
        /// <param name="option">The customization option</param>
        public void AddCustomizationOption(CustomizationOption option)
        {
            if (option != null && !CustomizationOptions.Contains(option))
            {
                CustomizationOptions.Add(option);
            }
        }

        /// <summary>
        /// Updates the item rating
        /// </summary>
        /// <param name="newRating">The new rating (1-5)</param>
        public void UpdateRating(double newRating)
        {
            if (newRating >= 1 && newRating <= 5)
            {
                Rating = newRating;
                ReviewCount++;
            }
        }

        /// <summary>
        /// Gets the display price with currency symbol
        /// </summary>
        /// <returns>Formatted price string</returns>
        public string GetDisplayPrice()
        {
            return $"£{Price:F2}";
        }

        /// <summary>
        /// Checks if item has any customization options
        /// </summary>
        /// <returns>True if customizable, false otherwise</returns>
        public bool IsCustomizable()
        {
            return CustomizationOptions.Count > 0;
        }
    }

    /// <summary>
    /// Represents a customization option for menu items
    /// </summary>
    public class CustomizationOption
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double AdditionalCost { get; set; }
        public bool IsRequired { get; set; }
        public List<string> Choices { get; set; } = new List<string>();
        public int MaxSelections { get; set; } = 1;

        public CustomizationOption(string name, double additionalCost = 0)
        {
            Name = name;
            AdditionalCost = additionalCost;
        }

        public override bool Equals(object? obj)
        {
            if (obj is CustomizationOption other)
            {
                return Name == other.Name;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
} 