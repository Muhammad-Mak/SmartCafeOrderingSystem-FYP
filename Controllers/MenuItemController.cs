using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartCafeOrderingSystem_Api_V2.Context;
using SmartCafeOrderingSystem_Api_V2.Models;

// Contains get all menu items
// get a specifc menu item
// create a new menu item
// update an existing menu item
// delete a menu item
// get popular items

namespace SmartCafeOrderingSystem_Api_V2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public MenuItemController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/MenuItem
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuItem>>> GetMenuItems()
        {
            return await _dbContext.MenuItems.Include(m => m.Category).ToListAsync();
        }

        // GET: api/MenuItem/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MenuItem>> GetMenuItem(int id)
        {
            var menuItem = await _dbContext.MenuItems.Include(m => m.Category)
                                                     .FirstOrDefaultAsync(m => m.ItemID == id);

            if (menuItem == null)
            {
                return NotFound(new { Message = "Menu item not found" });
            }

            return menuItem;
        }

        // POST: api/MenuItem
        [HttpPost]
        public async Task<ActionResult<MenuItem>> CreateMenuItem(MenuItem menuItem)
        {
            if (menuItem == null || string.IsNullOrWhiteSpace(menuItem.Name) || menuItem.Price <= 0)
            {
                return BadRequest(new { Message = "Invalid menu item data" });
            }

            var categoryExists = await _dbContext.MenuCategories.AnyAsync(c => c.CategoryID == menuItem.CategoryID);
            if (!categoryExists)
            {
                return BadRequest(new { Message = "Invalid category ID" });
            }

            _dbContext.MenuItems.Add(menuItem);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMenuItem), new { id = menuItem.ItemID }, menuItem);
        }

        // PUT: api/MenuItem/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMenuItem(int id, MenuItem menuItem)
        {
            if (id != menuItem.ItemID)
            {
                return BadRequest(new { Message = "Menu item ID mismatch" });
            }

            _dbContext.Entry(menuItem).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_dbContext.MenuItems.Any(e => e.ItemID == id))
                {
                    return NotFound(new { Message = "Menu item not found" });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/MenuItem/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            var menuItem = await _dbContext.MenuItems.FindAsync(id);
            if (menuItem == null)
            {
                return NotFound(new { Message = "Menu item not found" });
            }

            _dbContext.MenuItems.Remove(menuItem);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/MenuItem/popular
        [HttpGet("popular")]
        public async Task<ActionResult<IEnumerable<MenuItem>>> GetPopularMenuItems()
        {
            return await _dbContext.MenuItems.Where(m => m.IsPopular).ToListAsync();
        }
    }
}
