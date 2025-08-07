using System;
using System.Collections.Generic;
using System.Linq;
using OnlineOrderingSystem.Data;
using OnlineOrderingSystem.Models;

namespace OnlineOrderingSystem.Database
{
    /// <summary>
    /// Data access layer for menu items using Entity Framework Core
    /// </summary>
    public class MenuDataAccess
    {
        /// <summary>
        /// Retrieves all menu items from the database
        /// </summary>
        /// <returns>List of all menu items</returns>
        public List<Item> GetAllMenuItems()
        {
            using (var context = new OrderingDbContext())
            {
                return context.Items.ToList();
            }
        }

        /// <summary>
        /// Retrieves menu items by category
        /// </summary>
        /// <param name="category">The category to filter by</param>
        /// <returns>List of items in the specified category</returns>
        public List<Item> GetMenuItemsByCategory(string category)
        {
            using (var context = new OrderingDbContext())
            {
                return context.Items
                    .Where(item => item.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
        }

        /// <summary>
        /// Retrieves a specific menu item by ID
        /// </summary>
        /// <param name="itemId">The ID of the item</param>
        /// <returns>The menu item or null if not found</returns>
        public Item? GetMenuItemById(int itemId)
        {
            using (var context = new OrderingDbContext())
            {
                return context.Items.FirstOrDefault(item => item.Id == itemId);
            }
        }

        /// <summary>
        /// Retrieves available menu items
        /// </summary>
        /// <returns>List of available menu items</returns>
        public List<Item> GetAvailableMenuItems()
        {
            using (var context = new OrderingDbContext())
            {
                return context.Items.Where(item => item.Available).ToList();
            }
        }

        /// <summary>
        /// Searches menu items by name or description
        /// </summary>
        /// <param name="searchTerm">The search term</param>
        /// <returns>List of matching menu items</returns>
        public List<Item> SearchMenuItems(string searchTerm)
        {
            using (var context = new OrderingDbContext())
            {
                return context.Items
                    .Where(item => item.Name.Contains(searchTerm) || 
                                   item.Description.Contains(searchTerm))
                    .ToList();
            }
        }

        /// <summary>
        /// Adds a new menu item to the database
        /// </summary>
        /// <param name="item">The item to add</param>
        /// <returns>The added item with its generated ID</returns>
        public Item AddMenuItem(Item item)
        {
            using (var context = new OrderingDbContext())
            {
                context.Items.Add(item);
                context.SaveChanges();
                return item;
            }
        }

        /// <summary>
        /// Updates an existing menu item
        /// </summary>
        /// <param name="item">The item to update</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool UpdateMenuItem(Item item)
        {
            using (var context = new OrderingDbContext())
            {
                context.Items.Update(item);
                return context.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// Deletes a menu item by ID
        /// </summary>
        /// <param name="itemId">The ID of the item to delete</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool DeleteMenuItem(int itemId)
        {
            using (var context = new OrderingDbContext())
            {
                var item = context.Items.Find(itemId);
                if (item != null)
                {
                    context.Items.Remove(item);
                    return context.SaveChanges() > 0;
                }
                return false;
            }
        }

        /// <summary>
        /// Gets menu items by dietary requirements
        /// </summary>
        /// <param name="dietaryRequirement">The dietary requirement to filter by</param>
        /// <returns>List of items matching the dietary requirement</returns>
        public List<Item> GetMenuItemsByDietaryRequirement(string dietaryRequirement)
        {
            using (var context = new OrderingDbContext())
            {
                return context.Items
                    .Where(item => item.DietaryTags.Contains(dietaryRequirement))
                    .ToList();
            }
        }
    }
} 