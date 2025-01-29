using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartCafeOrderingSystem_Api_V2.Context;
using SmartCafeOrderingSystem_Api_V2.Models;

//Contains get all recommendations
// get recommendations for a specific item
// create a new recommendation
// update an existing recommendation
// delete a recommendation

namespace SmartCafeOrderingSystem_Api_V2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public RecommendationController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/Recommendation
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recommendation>>> GetRecommendations()
        {
            return await _dbContext.Recommendations
                .Include(r => r.Item1)
                .Include(r => r.Item2)
                .ToListAsync();
        }

        // GET: api/Recommendation/{itemId}
        [HttpGet("{itemId}")]
        public async Task<ActionResult<IEnumerable<MenuItem>>> GetRecommendationsForItem(int itemId)
        {
            var recommendations = await _dbContext.Recommendations
                .Where(r => r.ItemID1 == itemId || r.ItemID2 == itemId)
                .Include(r => r.Item1)
                .Include(r => r.Item2)
                .OrderByDescending(r => r.Score)
                .ToListAsync();

            if (!recommendations.Any())
            {
                return NotFound(new { Message = "No recommendations found for this item" });
            }

            // Return a list of recommended menu items
            var recommendedItems = recommendations.Select(r =>
                r.ItemID1 == itemId ? r.Item2 : r.Item1).Distinct().ToList();

            return recommendedItems;
        }

        // POST: api/Recommendation
        [HttpPost]
        public async Task<ActionResult<Recommendation>> CreateRecommendation(Recommendation recommendation)
        {
            if (recommendation == null || recommendation.ItemID1 == recommendation.ItemID2)
            {
                return BadRequest(new { Message = "Invalid recommendation data" });
            }

            var item1Exists = await _dbContext.MenuItems.AnyAsync(m => m.ItemID == recommendation.ItemID1);
            var item2Exists = await _dbContext.MenuItems.AnyAsync(m => m.ItemID == recommendation.ItemID2);

            if (!item1Exists || !item2Exists)
            {
                return BadRequest(new { Message = "Invalid menu item IDs" });
            }

            _dbContext.Recommendations.Add(recommendation);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRecommendationsForItem), new { itemId = recommendation.ItemID1 }, recommendation);
        }

        // PUT: api/Recommendation/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecommendation(int id, Recommendation recommendation)
        {
            if (id != recommendation.RecommendationID)
            {
                return BadRequest(new { Message = "Recommendation ID mismatch" });
            }

            _dbContext.Entry(recommendation).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_dbContext.Recommendations.Any(e => e.RecommendationID == id))
                {
                    return NotFound(new { Message = "Recommendation not found" });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Recommendation/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecommendation(int id)
        {
            var recommendation = await _dbContext.Recommendations.FindAsync(id);
            if (recommendation == null)
            {
                return NotFound(new { Message = "Recommendation not found" });
            }

            _dbContext.Recommendations.Remove(recommendation);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
