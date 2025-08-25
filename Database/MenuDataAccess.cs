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
                    .Where(item => item.Category.ToLower() == category.ToLower())
                    .ToList();
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

        
    }
} 