using System.Collections.Generic;
using System.Linq;
using TeamEnigma.Models;

namespace TeamEnigma.Services
{
    public class CartService
    {
        private readonly List<CartItem> _cartItems = new List<CartItem>();

        // Add an item to the cart
        public void AddToCart(Item item, int quantity)
        {
            var existingItem = _cartItems.FirstOrDefault(ci => ci.ItemId == item.Id);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                _cartItems.Add(new CartItem { Item = item, ItemId = item.Id, Quantity = quantity });
            }
        }

        // Remove an item from the cart
        public void RemoveFromCart(int itemId)
        {
            var cartItem = _cartItems.FirstOrDefault(ci => ci.ItemId == itemId);
            if (cartItem != null)
            {
                _cartItems.Remove(cartItem);
            }
        }

        // Get total price of all cart items
        public decimal GetTotalPrice()
        {
            return _cartItems.Sum(ci => ci.TotalPrice);
        }

        // Get all items in the cart
        public List<CartItem> GetCartItems()
        {
            return _cartItems;
        }
    }
}
