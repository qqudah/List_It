using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TeamEnigma.Data;
using TeamEnigma.Models;

namespace TeamEnigma.Controllers
{
    [Authorize]
    public class ItemsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ItemsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }


        // Common logic for search and category filtering
        private IQueryable<Item> ApplyFilters(string search, Category? category, decimal? minPrice, decimal? maxPrice)
        {
            var items = _context.Item.Include(i => i.User).AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                items = items.Where(i =>
                    i.Name.Contains(search) ||
                    i.Location.Contains(search)
                );
            }

            if (category.HasValue)
            {
                items = items.Where(i => i.Category == category.Value);
            }

            if (minPrice.HasValue)
            {
                items = items.Where(i => i.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                items = items.Where(i => i.Price <= maxPrice.Value);
            }

            return items;
        }

        // GET: Items/sell
        public async Task<IActionResult> Sell(string search, Category? category, decimal? minPrice, decimal? maxPrice)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge(); // Redirects to login if the user is not authenticated
            }

            // Apply filters and also ensure items belong to the logged-in user
            var userItems = ApplyFilters(search, category,minPrice,maxPrice)
                            .Where(i => i.UserId == user.Id); // Filter by user ID

            ViewBag.SelectedCategory = category;
            ViewBag.SearchQuery = search;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            return View(await userItems.ToListAsync());
        }

        // GET: Items
        public async Task<IActionResult> Index(string search, Category? category, decimal? minPrice, decimal? maxPrice)
        {
            var items = ApplyFilters(search, category,minPrice,maxPrice); // Apply filters for general items

            ViewBag.SelectedCategory = category;
            ViewBag.SearchQuery = search;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            return View(await items.ToListAsync());
        }


        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            // Retrieve the logged-in user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Pass the UserId to the view model or the view directly
            var model = new Item
            {
                UserId = userId
            };

            return View(model);
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,Status,ContactNumber,ImageUrl,Category,Location,UserId")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", item.UserId);
            return View(item);
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", item.UserId);
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,Status,ContactNumber,ImageUrl,Category,Location,UserId")] Item item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", item.UserId);
            return View(item);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Item.FindAsync(id);
            if (item != null)
            {
                _context.Item.Remove(item);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Sell));
        }

        private bool ItemExists(int id)
        {
            return _context.Item.Any(e => e.Id == id);
        }
    }
}
