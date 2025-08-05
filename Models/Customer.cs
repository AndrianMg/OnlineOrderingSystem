using System;
using System.Collections.Generic;
using System.Linq; // Added for FirstOrDefault

namespace OnlineOrderingSystem.Models
{
    /// <summary>
    /// Represents a customer in the online ordering system
    /// </summary>
    public class Customer
    {
        public int CustomerID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public List<Order> OrderHistory { get; set; } = new List<Order>();
        public string PreferredPaymentMethod { get; set; } = string.Empty;

        /// <summary>
        /// Registers a new customer in the system
        /// </summary>
        public void Register()
        {
            // Implementation for customer registration
            Console.WriteLine($"Customer {Name} registered successfully.");
        }

        /// <summary>
        /// Logs in the customer to the system
        /// </summary>
        public void Login()
        {
            // Implementation for customer login
            Console.WriteLine($"Customer {Name} logged in successfully.");
        }

        /// <summary>
        /// Updates the customer's address
        /// </summary>
        /// <param name="newAddress">The new address</param>
        public void UpdateAddress(string newAddress)
        {
            Address = newAddress;
            Console.WriteLine($"Address updated to: {newAddress}");
        }

        /// <summary>
        /// Retrieves the customer's order history
        /// </summary>
        /// <returns>List of orders</returns>
        public List<Order> ViewOrderHistory()
        {
            return OrderHistory;
        }

        /// <summary>
        /// Places a new order using the provided cart
        /// </summary>
        /// <param name="cart">The cart containing items to order</param>
        public void PlaceOrder(Cart cart)
        {
            if (cart.Items.Count == 0)
            {
                throw new InvalidOperationException("Cannot place order with empty cart");
            }

            var order = new Order
            {
                CustomerID = CustomerID,
                OrderItems = new List<OrderDetail>(),
                OrderDate = DateTime.Now,
                OrderStatus = "Pending"
            };

            foreach (var cartItem in cart.Items)
            {
                var orderDetail = new OrderDetail
                {
                    ItemID = cartItem.Item.ItemID,
                    ItemName = cartItem.Item.Name,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Item.Price,
                    TotalPrice = cartItem.TotalPrice
                };
                order.OrderItems.Add(orderDetail);
            }

            order.CalculateTotal();
            OrderHistory.Add(order);
            Console.WriteLine($"Order placed successfully. Total: ${order.TotalAmount}");
        }

        /// <summary>
        /// Cancels an existing order
        /// </summary>
        /// <param name="orderID">The ID of the order to cancel</param>
        public void CancelOrder(int orderID)
        {
            var order = OrderHistory.FirstOrDefault(o => o.OrderID == orderID);
            if (order != null)
            {
                order.CancelOrder();
                Console.WriteLine($"Order {orderID} cancelled successfully.");
            }
            else
            {
                throw new ArgumentException($"Order {orderID} not found.");
            }
        }

        /// <summary>
        /// Adds an item to the customer's cart
        /// </summary>
        /// <param name="item">The item to add</param>
        /// <param name="quantity">The quantity to add</param>
        public void AddToCart(Item item, int quantity)
        {
            // Implementation would typically work with a cart service
            Console.WriteLine($"Added {quantity} of {item.Name} to cart.");
        }

        /// <summary>
        /// Removes an item from the customer's cart
        /// </summary>
        /// <param name="item">The item to remove</param>
        /// <param name="quantity">The quantity to remove</param>
        public void RemoveFromCart(Item item, int quantity)
        {
            // Implementation would typically work with a cart service
            Console.WriteLine($"Removed {quantity} of {item.Name} from cart.");
        }

        /// <summary>
        /// Views the current cart contents
        /// </summary>
        /// <returns>The current cart</returns>
        public Cart ViewCart()
        {
            // Implementation would typically retrieve from a cart service
            return new Cart { CustomerID = CustomerID };
        }
    }
}
