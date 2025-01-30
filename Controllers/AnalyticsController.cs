using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartCafeOrderingSystem_Api_V2.Context;
using SmartCafeOrderingSystem_Api_V2.DTOs;
using SmartCafeOrderingSystem_Api_V2.Models;

//Contains get all analyticss data
// get latest analytics entry
// get total sales
// get total orders
// get popular items(top 5 items)
// create an analytics entry
// delete an analytics entry

namespace SmartCafeOrderingSystem_Api_V2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public AnalyticsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/Analytics
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnalyticsDTO>>> GetAnalyticsData()
        {
            var analyticsData = await _dbContext.Analytics
                .OrderByDescending(a => a.Date)
                .Select(a => new AnalyticsDTO
                {
                    AnalyticsID = a.AnalyticsID,
                    Date = a.Date,
                    TotalSales = a.TotalSales,
                    TotalOrders = a.TotalOrders,
                    PopularItems = a.PopularItems
                }).ToListAsync();

            return Ok(analyticsData);
        }

        // GET: api/Analytics/latest
        [HttpGet("latest")]
        public async Task<ActionResult<AnalyticsDTO>> GetLatestAnalytics()
        {
            var latestAnalytics = await _dbContext.Analytics
                .OrderByDescending(a => a.Date)
                .Select(a => new AnalyticsDTO
                {
                    AnalyticsID = a.AnalyticsID,
                    Date = a.Date,
                    TotalSales = a.TotalSales,
                    TotalOrders = a.TotalOrders,
                    PopularItems = a.PopularItems
                })
                .FirstOrDefaultAsync();

            if (latestAnalytics == null)
            {
                return NotFound(new { Message = "No analytics data found" });
            }

            return Ok(latestAnalytics);
        }

        // GET: api/Analytics/totalsales
        [HttpGet("totalsales")]
        public async Task<ActionResult<decimal>> GetTotalSales()
        {
            decimal totalSales = await _dbContext.Orders
                .Where(o => o.PaymentStatus == "Paid")
                .SumAsync(o => o.TotalAmount);

            return Ok(new { TotalSales = totalSales });
        }

        // GET: api/Analytics/totalorders
        [HttpGet("totalorders")]
        public async Task<ActionResult<int>> GetTotalOrders()
        {
            int totalOrders = await _dbContext.Orders.CountAsync();
            return Ok(new { TotalOrders = totalOrders });
        }

        // GET: api/Analytics/popularitems
        [HttpGet("popularitems")]
        public async Task<ActionResult<IEnumerable<object>>> GetPopularItems()
        {
            var popularItems = await _dbContext.OrderItems
                .GroupBy(oi => oi.ItemID)
                .Select(g => new
                {
                    ItemID = g.Key,
                    Count = g.Count(),
                    MenuItem = _dbContext.MenuItems.Where(m => m.ItemID == g.Key)
                                                   .Select(m => new { m.Name, m.Price, m.ImageURL })
                                                   .FirstOrDefault()
                })
                .OrderByDescending(i => i.Count)
                .Take(5)
                .ToListAsync();

            return Ok(popularItems);
        }

        // POST: api/Analytics
        [HttpPost]
        public async Task<ActionResult<AnalyticsDTO>> CreateAnalytics([FromBody] AnalyticsDTO analyticsDTO)
        {
            if (analyticsDTO == null)
            {
                return BadRequest(new { Message = "Invalid analytics data" });
            }

            var analytics = new Analytics
            {
                Date = analyticsDTO.Date,
                TotalSales = analyticsDTO.TotalSales,
                TotalOrders = analyticsDTO.TotalOrders,
                PopularItems = analyticsDTO.PopularItems
            };

            _dbContext.Analytics.Add(analytics);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLatestAnalytics), new { id = analytics.AnalyticsID }, analyticsDTO);
        }

        // DELETE: api/Analytics/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnalytics(int id)
        {
            var analytics = await _dbContext.Analytics.FindAsync(id);
            if (analytics == null)
            {
                return NotFound(new { Message = "Analytics data not found" });
            }

            _dbContext.Analytics.Remove(analytics);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
