using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartCafeOrderingSystem_Api_V2.Context;
using SmartCafeOrderingSystem_Api_V2.DTOs;
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
        public async Task<ActionResult<IEnumerable<MenuItemDTO>>> GetMenuItems()
        {
            var menuItems = await _dbContext.MenuItems.Include(m => m.Category)
                .Select(m => new MenuItemDTO
                {
                    ItemID = m.ItemID,
                    Name = m.Name,
                    Description = m.Description,
                    Price = m.Price,
                    ImageURL = m.ImageURL,
                    IsPopular = m.IsPopular,
                    CreatedDate = m.CreatedDate,
                    CategoryID = m.CategoryID,
                    CategoryName = m.Category.CategoryName
                }).ToListAsync();

            return Ok(menuItems);
        }

        // GET: api/MenuItem/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MenuItemDTO>> GetMenuItem(int id)
        {
            var menuItem = await _dbContext.MenuItems.Include(m => m.Category)
                .Where(m => m.ItemID == id)
                .Select(m => new MenuItemDTO
                {
                    ItemID = m.ItemID,
                    Name = m.Name,
                    Description = m.Description,
                    Price = m.Price,
                    ImageURL = m.ImageURL,
                    IsPopular = m.IsPopular,
                    CreatedDate = m.CreatedDate,
                    CategoryID = m.CategoryID,
                    CategoryName = m.Category.CategoryName
                }).FirstOrDefaultAsync();

            if (menuItem == null)
            {
                return NotFound(new { Message = "Menu item not found" });
            }

            return Ok(menuItem);
        }

        // POST: api/MenuItem
        [HttpPost]
        public async Task<ActionResult<MenuItemDTO>> CreateMenuItem([FromBody] MenuItemDTO menuItemDTO)
        {
            if (menuItemDTO == null || string.IsNullOrWhiteSpace(menuItemDTO.Name) || menuItemDTO.Price <= 0)
            {
                return BadRequest(new { Message = "Invalid menu item data" });
            }

            var categoryExists = await _dbContext.MenuCategories.AnyAsync(c => c.CategoryID == menuItemDTO.CategoryID);
            if (!categoryExists)
            {
                return BadRequest(new { Message = "Invalid category ID" });
            }

            var menuItem = new MenuItem
            {
                Name = menuItemDTO.Name,
                Description = menuItemDTO.Description,
                Price = menuItemDTO.Price,
                ImageURL = menuItemDTO.ImageURL,
                IsPopular = menuItemDTO.IsPopular,
                CreatedDate = DateTime.UtcNow,
                CategoryID = menuItemDTO.CategoryID
            };

            _dbContext.MenuItems.Add(menuItem);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMenuItem), new { id = menuItem.ItemID }, menuItemDTO);
        }

        // PUT: api/MenuItem/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMenuItem(int id, [FromBody] MenuItemDTO menuItemDTO)
        {
            var menuItem = await _dbContext.MenuItems.FindAsync(id);
            if (menuItem == null)
            {
                return NotFound(new { Message = "Menu item not found" });
            }

            menuItem.Name = menuItemDTO.Name;
            menuItem.Description = menuItemDTO.Description;
            menuItem.Price = menuItemDTO.Price;
            menuItem.ImageURL = menuItemDTO.ImageURL;
            menuItem.IsPopular = menuItemDTO.IsPopular;
            menuItem.CategoryID = menuItemDTO.CategoryID;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { Message = "Concurrency conflict occurred while updating the menu item" });
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
        public async Task<ActionResult<IEnumerable<MenuItemDTO>>> GetPopularMenuItems()
        {
            var popularItems = await _dbContext.MenuItems.Where(m => m.IsPopular)
                .Select(m => new MenuItemDTO
                {
                    ItemID = m.ItemID,
                    Name = m.Name,
                    Description = m.Description,
                    Price = m.Price,
                    ImageURL = m.ImageURL,
                    IsPopular = m.IsPopular,
                    CreatedDate = m.CreatedDate,
                    CategoryID = m.CategoryID,
                    CategoryName = m.Category.CategoryName
                }).ToListAsync();

            return Ok(popularItems);
        }
    }
}
