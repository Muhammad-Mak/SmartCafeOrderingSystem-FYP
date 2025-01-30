using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartCafeOrderingSystem_Api_V2.Context;
using SmartCafeOrderingSystem_Api_V2.DTOs;
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
        public async Task<ActionResult<IEnumerable<MenuCategoryDTO>>> GetMenuCategories()
        {
            var categories = await _dbContext.MenuCategories
                .Select(c => new MenuCategoryDTO
                {
                    CategoryID = c.CategoryID,
                    CategoryName = c.CategoryName,
                    Description = c.Description
                }).ToListAsync();

            return Ok(categories);
        }

        // GET: api/MenuCategory/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MenuCategoryDTO>> GetMenuCategory(int id)
        {
            var category = await _dbContext.MenuCategories
                .Where(c => c.CategoryID == id)
                .Select(c => new MenuCategoryDTO
                {
                    CategoryID = c.CategoryID,
                    CategoryName = c.CategoryName,
                    Description = c.Description
                }).FirstOrDefaultAsync();

            if (category == null)
            {
                return NotFound(new { Message = "Menu Category not found" });
            }

            return Ok(category);
        }

        // POST: api/MenuCategory
        [HttpPost]
        public async Task<ActionResult<MenuCategoryDTO>> CreateMenuCategory([FromBody] MenuCategoryDTO categoryDTO)
        {
            if (categoryDTO == null || string.IsNullOrWhiteSpace(categoryDTO.CategoryName))
            {
                return BadRequest(new { Message = "Invalid category data" });
            }

            var category = new MenuCategory
            {
                CategoryName = categoryDTO.CategoryName,
                Description = categoryDTO.Description
            };

            _dbContext.MenuCategories.Add(category);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMenuCategory), new { id = category.CategoryID }, categoryDTO);
        }

        // PUT: api/MenuCategory/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMenuCategory(int id, [FromBody] MenuCategoryDTO categoryDTO)
        {
            var category = await _dbContext.MenuCategories.FindAsync(id);
            if (category == null)
            {
                return NotFound(new { Message = "Menu Category not found" });
            }

            category.CategoryName = categoryDTO.CategoryName;
            category.Description = categoryDTO.Description;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { Message = "Concurrency conflict occurred while updating the category" });
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
