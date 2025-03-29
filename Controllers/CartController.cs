using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TeamEnigma.Data;
using TeamEnigma.Models;
using TeamEnigma.Services;

namespace TeamEnigma.Controllers
{
    public class CartController : Controller
    {
        private readonly CartService _cartService;
        private readonly ApplicationDbContext _context; // Database context

        public CartController(CartService cartService, ApplicationDbContext context)
        {
            _cartService = cartService;
            _context = context;
        }

        // Add an item to the cart
        [HttpPost]
        public IActionResult AddToCart(int itemId)
        {
            var item = _context.Item.Find(itemId);
            if (item != null)
            {
                _cartService.AddToCart(item, 1); // Adds 1 quantity by default
            }
            return RedirectToAction("Index");
        }

        // Remove an item from the cart
        [HttpPost]
        public IActionResult RemoveFromCart(int itemId)
        {
            _cartService.RemoveFromCart(itemId);
            return RedirectToAction("Index");
        }

        // Display the cart
        public IActionResult Index()
        {
            var cartItems = _cartService.GetCartItems();
            return View(cartItems);
        }
    }
}
