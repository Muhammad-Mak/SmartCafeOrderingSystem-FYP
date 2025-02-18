﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartCafeOrderingSystem_Api_V2.Context;
using SmartCafeOrderingSystem_Api_V2.DTOs;
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
        public async Task<ActionResult<IEnumerable<RecommendationDTO>>> GetRecommendations()
        {
            var recommendations = await _dbContext.Recommendations
                .Include(r => r.Item1)
                .Include(r => r.Item2)
                .Select(r => new RecommendationDTO
                {
                    RecommendationID = r.RecommendationID,
                    ItemID1 = r.ItemID1,
                    ItemName1 = r.Item1.Name,
                    ItemID2 = r.ItemID2,
                    ItemName2 = r.Item2.Name,
                    Score = r.Score
                }).ToListAsync();

            return Ok(recommendations);
        }

        // GET: api/Recommendation/{itemId}
        [HttpGet("{itemId}")]
        public async Task<ActionResult<IEnumerable<RecommendationDTO>>> GetRecommendationsForItem(int itemId)
        {
            var recommendations = await _dbContext.Recommendations
                .Where(r => r.ItemID1 == itemId || r.ItemID2 == itemId)
                .Include(r => r.Item1)
                .Include(r => r.Item2)
                .OrderByDescending(r => r.Score)
                .Select(r => new RecommendationDTO
                {
                    RecommendationID = r.RecommendationID,
                    ItemID1 = r.ItemID1,
                    ItemName1 = r.Item1.Name,
                    ItemID2 = r.ItemID2,
                    ItemName2 = r.Item2.Name,
                    Score = r.Score
                }).ToListAsync();

            if (!recommendations.Any())
            {
                return NotFound(new { Message = "No recommendations found for this item" });
            }

            return Ok(recommendations);
        }

        // POST: api/Recommendation
        [HttpPost]
        public async Task<ActionResult<RecommendationDTO>> CreateRecommendation([FromBody] RecommendationDTO recommendationDTO)
        {
            if (recommendationDTO == null || recommendationDTO.ItemID1 == recommendationDTO.ItemID2)
            {
                return BadRequest(new { Message = "Invalid recommendation data" });
            }

            var item1Exists = await _dbContext.MenuItems.AnyAsync(m => m.ItemID == recommendationDTO.ItemID1);
            var item2Exists = await _dbContext.MenuItems.AnyAsync(m => m.ItemID == recommendationDTO.ItemID2);

            if (!item1Exists || !item2Exists)
            {
                return BadRequest(new { Message = "Invalid menu item IDs" });
            }

            var recommendation = new Recommendation
            {
                ItemID1 = recommendationDTO.ItemID1,
                ItemID2 = recommendationDTO.ItemID2,
                Score = recommendationDTO.Score
            };

            _dbContext.Recommendations.Add(recommendation);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRecommendationsForItem), new { itemId = recommendation.ItemID1 }, recommendationDTO);
        }

        // PUT: api/Recommendation/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecommendation(int id, [FromBody] RecommendationDTO recommendationDTO)
        {
            var recommendation = await _dbContext.Recommendations.FindAsync(id);
            if (recommendation == null)
            {
                return NotFound(new { Message = "Recommendation not found" });
            }

            recommendation.Score = recommendationDTO.Score;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { Message = "Concurrency conflict occurred while updating the recommendation" });
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
