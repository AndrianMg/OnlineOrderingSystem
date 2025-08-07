using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineOrderingSystem.Models
{
    /// <summary>
    /// Represents a shopping cart for the online ordering system
    /// Manages a collection of items that a customer wants to purchase
    /// </summary>
    public class Cart
    {
        /// <summary>
        /// List of items in the cart
        /// </summary>
        private List<Item> items;

        // Legacy properties for backward compatibility
        /// <summary>
        /// Cart ID for database operations
        /// </summary>
        public int CartID { get; set; }

        /// <summary>
        /// Customer ID associated with this cart
        /// </summary>
        public int CustomerID { get; set; }

        /// <summary>
        /// List of cart items with quantities
        /// </summary>
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        /// <summary>
        /// Total amount of the cart
        /// </summary>
        public double TotalAmount { get; set; }

        /// <summary>
        /// Date when cart was created
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Date when cart was last updated
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// Default constructor - initializes an empty cart
        /// </summary>
        public Cart()
        {
            items = new List<Item>();
            CreateDate = DateTime.Now;
            UpdateDate = DateTime.Now;
        }

        /// <summary>
        /// Constructor that initializes cart with existing items
        /// </summary>
        /// <param name="initialItems">List of items to add to the cart</param>
        public Cart(List<Item> initialItems)
        {
            items = initialItems ?? new List<Item>();
            CreateDate = DateTime.Now;
            UpdateDate = DateTime.Now;
        }

        /// <summary>
        /// Gets all items currently in the cart
        /// </summary>
        /// <returns>List of items in the cart</returns>
        public List<Item> GetItems()
        {
            return new List<Item>(items); // Return a copy to prevent external modification
        }

        /// <summary>
        /// Adds an item to the cart
        /// </summary>
        /// <param name="item">Item to add to the cart</param>
        public void AddItem(Item item)
        {
            if (item != null && item.Available)
            {
                items.Add(item);
                UpdateDate = DateTime.Now;
            }
        }

        /// <summary>
        /// Adds an item to the cart with quantity (legacy method)
        /// </summary>
        /// <param name="item">Item to add</param>
        /// <param name="quantity">Quantity to add</param>
        public void AddItem(Item item, int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero");
            }

            // Check if item already exists in cart
            var existingCartItem = Items.FirstOrDefault(i => i.Item.ItemID == item.ItemID);
            if (existingCartItem != null)
            {
                // Update quantity of existing item
                existingCartItem.Quantity += quantity;
            }
            else
            {
                // Add new item to cart
                Items.Add(new CartItem(item, quantity));
            }

            UpdateDate = DateTime.Now;
            CalculateTotal();
        }

        /// <summary>
        /// Removes a specific item from the cart
        /// </summary>
        /// <param name="item">Item to remove from the cart</param>
        public void RemoveItem(Item item)
        {
            if (item != null)
            {
                items.Remove(item);
                UpdateDate = DateTime.Now;
            }
        }

        /// <summary>
        /// Removes an item from the cart by ID (legacy method)
        /// </summary>
        /// <param name="itemID">The ID of the item to remove</param>
        public void RemoveItem(int itemID)
        {
            var cartItem = Items.FirstOrDefault(i => i.Item.ItemID == itemID);
            if (cartItem != null)
            {
                Items.Remove(cartItem);
                UpdateDate = DateTime.Now;
                CalculateTotal();
            }
        }

        /// <summary>
        /// Updates the quantity of an item in the cart (legacy method)
        /// </summary>
        /// <param name="itemID">The ID of the item</param>
        /// <param name="newQuantity">The new quantity</param>
        public void UpdateItemQuantity(int itemID, int newQuantity)
        {
            if (newQuantity <= 0)
            {
                RemoveItem(itemID);
                return;
            }

            var cartItem = Items.FirstOrDefault(i => i.Item.ItemID == itemID);
            if (cartItem != null)
            {
                cartItem.Quantity = newQuantity;
                UpdateDate = DateTime.Now;
                CalculateTotal();
            }
        }

        /// <summary>
        /// Removes all items with a specific name from the cart
        /// </summary>
        /// <param name="itemName">Name of items to remove</param>
        public void RemoveItemsByName(string itemName)
        {
            if (!string.IsNullOrWhiteSpace(itemName))
            {
                items.RemoveAll(item => string.Equals(item.Name, itemName, StringComparison.OrdinalIgnoreCase));
                UpdateDate = DateTime.Now;
            }
        }

        /// <summary>
        /// Clears all items from the cart
        /// </summary>
        public void Clear()
        {
            items.Clear();
            Items.Clear();
            TotalAmount = 0;
            UpdateDate = DateTime.Now;
        }

        /// <summary>
        /// Gets the total number of items in the cart
        /// </summary>
        /// <returns>Total count of items</returns>
        public int GetItemCount()
        {
            return items.Count;
        }

        /// <summary>
        /// Gets the total price of all items in the cart
        /// </summary>
        /// <returns>Total price in pounds sterling</returns>
        public double GetTotal()
        {
            return Items.Sum(cartItem => cartItem.TotalPrice);
        }

        /// <summary>
        /// Calculates the total amount for all items in the cart (legacy method)
        /// </summary>
        /// <returns>The total amount</returns>
        public double CalculateTotal()
        {
            TotalAmount = Items.Sum(cartItem => cartItem.TotalPrice);
            return TotalAmount;
        }

        /// <summary>
        /// Gets the total price including tax
        /// </summary>
        /// <param name="taxRate">Tax rate as a decimal (e.g., 0.20 for 20%)</param>
        /// <returns>Total price including tax</returns>
        public double GetTotalWithTax(double taxRate = 0.0)
        {
            return GetTotal() * (1 + taxRate);
        }

        /// <summary>
        /// Checks if the cart is empty
        /// </summary>
        /// <returns>True if cart has no items</returns>
        public bool IsEmpty()
        {
            return Items.Count == 0;
        }

        /// <summary>
        /// Gets the number of unique items in the cart
        /// </summary>
        /// <returns>Count of unique items</returns>
        public int GetUniqueItemCount()
        {
            return items.Select(item => item.Name).Distinct().Count();
        }

        /// <summary>
        /// Gets a summary of items grouped by name with quantities
        /// </summary>
        /// <returns>Dictionary with item names as keys and quantities as values</returns>
        public Dictionary<string, int> GetItemSummary()
        {
            return items.GroupBy(item => item.Name)
                       .ToDictionary(group => group.Key, group => group.Count());
        }

        /// <summary>
        /// Gets the most expensive item in the cart
        /// </summary>
        /// <returns>The item with the highest price, or null if cart is empty</returns>
        public Item? GetMostExpensiveItem()
        {
            return items.OrderByDescending(item => item.Price).FirstOrDefault();
        }

        /// <summary>
        /// Gets the least expensive item in the cart
        /// </summary>
        /// <returns>The item with the lowest price, or null if cart is empty</returns>
        public Item? GetLeastExpensiveItem()
        {
            return items.OrderBy(item => item.Price).FirstOrDefault();
        }

        /// <summary>
        /// Gets the average price of items in the cart
        /// </summary>
        /// <returns>Average price, or 0 if cart is empty</returns>
        public double GetAveragePrice()
        {
            if (items.Count == 0)
                return 0.0;

            return items.Average(item => item.Price);
        }

        /// <summary>
        /// Checks if the cart contains any items from a specific category
        /// </summary>
        /// <param name="category">Category to check for</param>
        /// <returns>True if cart contains items from the specified category</returns>
        public bool ContainsCategory(string category)
        {
            return items.Any(item => string.Equals(item.Category, category, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Gets all items from a specific category
        /// </summary>
        /// <param name="category">Category to filter by</param>
        /// <returns>List of items from the specified category</returns>
        public List<Item> GetItemsByCategory(string category)
        {
            return items.Where(item => string.Equals(item.Category, category, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        /// <summary>
        /// Gets the total preparation time for all items in the cart
        /// </summary>
        /// <returns>Total preparation time in minutes</returns>
        public int GetTotalPreparationTime()
        {
            return items.Sum(item => item.PrepTime);
        }

        /// <summary>
        /// Gets the estimated delivery time based on preparation time
        /// </summary>
        /// <param name="baseDeliveryTime">Base delivery time in minutes</param>
        /// <returns>Estimated delivery time in minutes</returns>
        public int GetEstimatedDeliveryTime(int baseDeliveryTime = 30)
        {
            return baseDeliveryTime + GetTotalPreparationTime();
        }

        /// <summary>
        /// Applies a discount to the cart total
        /// </summary>
        /// <param name="discountPercentage">Discount percentage (0.0 to 1.0)</param>
        /// <returns>Discounted total price</returns>
        public double ApplyDiscount(double discountPercentage)
        {
            if (discountPercentage < 0 || discountPercentage > 1)
            {
                throw new ArgumentException("Discount percentage must be between 0 and 1");
            }

            return GetTotal() * (1 - discountPercentage);
        }

        /// <summary>
        /// Validates that all items in the cart are available
        /// </summary>
        /// <returns>True if all items are available</returns>
        public bool ValidateAvailability()
        {
            return items.All(item => item.Available);
        }

        /// <summary>
        /// Gets a formatted string representation of the cart
        /// </summary>
        /// <returns>Formatted cart summary</returns>
        public override string ToString()
        {
            if (IsEmpty())
            {
                return "Cart is empty";
            }

            var summary = GetItemSummary();
            var itemList = string.Join(", ", summary.Select(kvp => $"{kvp.Value}x {kvp.Key}"));
            return $"Cart ({GetItemCount()} items): {itemList} - Total: £{GetTotal():F2}";
        }

        /// <summary>
        /// Creates a copy of the current cart
        /// </summary>
        /// <returns>New cart with the same items</returns>
        public Cart Clone()
        {
            return new Cart(new List<Item>(items));
        }

        /// <summary>
        /// Merges another cart into this cart
        /// </summary>
        /// <param name="otherCart">Cart to merge</param>
        public void Merge(Cart otherCart)
        {
            if (otherCart != null)
            {
                items.AddRange(otherCart.GetItems());
                UpdateDate = DateTime.Now;
            }
        }

        /// <summary>
        /// Gets the cart details (legacy method)
        /// </summary>
        /// <returns>List of items in the cart</returns>
        public List<CartItem> GetCartDetails()
        {
            return Items;
        }

        /// <summary>
        /// Views the cart contents (legacy method)
        /// </summary>
        /// <returns>The current cart</returns>
        public Cart ViewCart()
        {
            return this;
        }
    }

    /// <summary>
    /// Represents a cart item with quantity
    /// </summary>
    public class CartItem
    {
        public int CartID { get; set; }
        public int ItemID { get; set; }
        public Item Item { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice => Item?.Price * Quantity ?? 0;

        public CartItem()
        {
            Item = new Item();
        }

        public CartItem(Item item, int quantity)
        {
            Item = item;
            ItemID = item.Id;
            Quantity = quantity;
        }
    }
} 