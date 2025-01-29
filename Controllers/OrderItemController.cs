using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartCafeOrderingSystem_Api_V2.Context;
using SmartCafeOrderingSystem_Api_V2.Models;

//Contains get all order items
// get a specific order item
// create a new order item
// update an existing order item
// delete an order item

namespace SmartCafeOrderingSystem_Api_V2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public OrderItemController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/OrderItem
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrderItems()
        {
            return await _dbContext.OrderItems
                .Include(oi => oi.MenuItem)
                .Include(oi => oi.Order)
                .ToListAsync();
        }

        // GET: api/OrderItem/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItem>> GetOrderItem(int id)
        {
            var orderItem = await _dbContext.OrderItems
                .Include(oi => oi.MenuItem)
                .Include(oi => oi.Order)
                .FirstOrDefaultAsync(oi => oi.OrderItemID == id);

            if (orderItem == null)
            {
                return NotFound(new { Message = "Order item not found" });
            }

            return orderItem;
        }

        // POST: api/OrderItem
        [HttpPost]
        public async Task<ActionResult<OrderItem>> CreateOrderItem(OrderItem orderItem)
        {
            if (orderItem == null || orderItem.Quantity <= 0)
            {
                return BadRequest(new { Message = "Invalid order item data" });
            }

            var menuItem = await _dbContext.MenuItems.FindAsync(orderItem.ItemID);
            if (menuItem == null)
            {
                return BadRequest(new { Message = "Invalid menu item ID" });
            }

            var order = await _dbContext.Orders.FindAsync(orderItem.OrderID);
            if (order == null)
            {
                return BadRequest(new { Message = "Invalid order ID" });
            }

            // Calculate subtotal
            orderItem.Subtotal = menuItem.Price * orderItem.Quantity;

            _dbContext.OrderItems.Add(orderItem);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrderItem), new { id = orderItem.OrderItemID }, orderItem);
        }

        // PUT: api/OrderItem/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderItem(int id, OrderItem orderItem)
        {
            if (id != orderItem.OrderItemID)
            {
                return BadRequest(new { Message = "Order item ID mismatch" });
            }

            _dbContext.Entry(orderItem).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_dbContext.OrderItems.Any(e => e.OrderItemID == id))
                {
                    return NotFound(new { Message = "Order item not found" });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/OrderItem/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            var orderItem = await _dbContext.OrderItems.FindAsync(id);
            if (orderItem == null)
            {
                return NotFound(new { Message = "Order item not found" });
            }

            _dbContext.OrderItems.Remove(orderItem);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
