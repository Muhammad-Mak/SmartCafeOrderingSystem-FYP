using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartCafeOrderingSystem_Api_V2.Context;
using SmartCafeOrderingSystem_Api_V2.Models;

// Contains Get all categories
// Get a specific category
// Create a new Category
// Update an existing category
// Delete a categroy

namespace SmartCafeOrderingSystem_Api_V2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuCategoryController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public MenuCategoryController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/MenuCategory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuCategory>>> GetMenuCategories()
        {
            return await _dbContext.MenuCategories.ToListAsync();
        }

        // GET: api/MenuCategory/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MenuCategory>> GetMenuCategory(int id)
        {
            var category = await _dbContext.MenuCategories.FindAsync(id);

            if (category == null)
            {
                return NotFound(new { Message = "Menu Category not found" });
            }

            return category;
        }

        // POST: api/MenuCategory
        [HttpPost]
        public async Task<ActionResult<MenuCategory>> CreateMenuCategory(MenuCategory category)
        {
            if (category == null || string.IsNullOrWhiteSpace(category.CategoryName))
            {
                return BadRequest(new { Message = "Invalid category data" });
            }

            _dbContext.MenuCategories.Add(category);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMenuCategory), new { id = category.CategoryID }, category);
        }

        // PUT: api/MenuCategory/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMenuCategory(int id, MenuCategory category)
        {
            if (id != category.CategoryID)
            {
                return BadRequest(new { Message = "Category ID mismatch" });
            }

            _dbContext.Entry(category).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_dbContext.MenuCategories.Any(e => e.CategoryID == id))
                {
                    return NotFound(new { Message = "Menu Category not found" });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/MenuCategory/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenuCategory(int id)
        {
            var category = await _dbContext.MenuCategories.FindAsync(id);
            if (category == null)
            {
                return NotFound(new { Message = "Menu Category not found" });
            }

            _dbContext.MenuCategories.Remove(category);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
// Contains Get all categories
// Get a specific category
// Create a new Category
// Update an existing category
//Delete a categroy
