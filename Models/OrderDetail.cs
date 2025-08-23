using System;
using System.Collections.Generic;

namespace OnlineOrderingSystem.Models
{
    /// <summary>
    /// Represents a detail line item within an order in Tasty Eats
    /// </summary>
    public class OrderDetail
    {
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double TotalPrice { get; set; }
        public string SpecialInstructions { get; set; } = string.Empty;
        
        // New properties for Tasty Eats
        public List<string> Customizations { get; set; } = new List<string>();
        
        // Database column for customizations (stored as JSON string)
        public string CustomizationsJson { get; set; } = "[]";
        public double CustomizationCost { get; set; }
        public string DietaryNotes { get; set; } = string.Empty;
        public bool IsSpicy { get; set; }
        public string AllergenInfo { get; set; } = string.Empty;

        /// <summary>
        /// Calculates the total price for this order detail including customizations
        /// </summary>
        /// <returns>The total price</returns>
        public double CalculateTotalPrice()
        {
            TotalPrice = (Price + CustomizationCost) * Quantity;
            return TotalPrice;
        }

        /// <summary>
        /// Gets the detail information as a string
        /// </summary>
        /// <returns>Formatted detail information</returns>
        public string GetDetail()
        {
            var detail = $"{ItemName} x{Quantity} @ £{Price:F2}";
            
            if (CustomizationCost > 0)
            {
                detail += $" (+£{CustomizationCost:F2} customizations)";
            }
            
            detail += $" = £{TotalPrice:F2}";
            
            if (Customizations.Count > 0)
            {
                detail += $"\n  Customizations: {string.Join(", ", Customizations)}";
            }
            
            if (!string.IsNullOrEmpty(SpecialInstructions))
            {
                detail += $"\n  Special Instructions: {SpecialInstructions}";
            }
            
            return detail;
        }

        /// <summary>
        /// Removes this item from the order
        /// </summary>
        public void RemoveItem()
        {
            // This would typically be handled by the parent Order class
            // Implementation here is for demonstration
            Quantity = 0;
            TotalPrice = 0;
        }

        /// <summary>
        /// Updates the quantity for this order detail
        /// </summary>
        /// <param name="newQuantity">The new quantity</param>
        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity < 0)
            {
                throw new ArgumentException("Quantity cannot be negative");
            }

            Quantity = newQuantity;
            CalculateTotalPrice();
        }

        /// <summary>
        /// Adds a customization to this order detail
        /// </summary>
        /// <param name="customization">The customization to add</param>
        /// <param name="additionalCost">Additional cost for the customization</param>
        public void AddCustomization(string customization, double additionalCost = 0)
        {
            if (!string.IsNullOrWhiteSpace(customization))
            {
                Customizations.Add(customization);
                CustomizationCost += additionalCost;
                CalculateTotalPrice();
            }
        }

        /// <summary>
        /// Removes a customization from this order detail
        /// </summary>
        /// <param name="customization">The customization to remove</param>
        public void RemoveCustomization(string customization)
        {
            if (Customizations.Contains(customization))
            {
                Customizations.Remove(customization);
                // Note: In a real implementation, you'd need to track the cost per customization
                CalculateTotalPrice();
            }
        }

        /// <summary>
        /// Gets a summary of the order detail
        /// </summary>
        /// <returns>Brief summary string</returns>
        public string GetSummary()
        {
            var summary = $"{ItemName} x{Quantity}";
            if (Customizations.Count > 0)
            {
                summary += $" ({string.Join(", ", Customizations)})";
            }
            return summary;
        }

        /// <summary>
        /// Checks if this item has any customizations
        /// </summary>
        /// <returns>True if customized, false otherwise</returns>
        public bool HasCustomizations()
        {
            return Customizations.Count > 0;
        }

        /// <summary>
        /// Gets the base price without customizations
        /// </summary>
        /// <returns>The base price</returns>
        public double GetBasePrice()
        {
            return Price * Quantity;
        }

        /// <summary>
        /// Gets the total customization cost
        /// </summary>
        /// <returns>The customization cost</returns>
        public double GetCustomizationCost()
        {
            return CustomizationCost * Quantity;
        }
    }
} 