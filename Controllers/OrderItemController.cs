using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartCafeOrderingSystem_Api_V2.Context;
using SmartCafeOrderingSystem_Api_V2.DTOs;
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
        public async Task<ActionResult<IEnumerable<OrderItemDTO>>> GetOrderItems()
        {
            var orderItems = await _dbContext.OrderItems
                .Include(oi => oi.MenuItem)
                .Include(oi => oi.Order)
                .Select(oi => new OrderItemDTO
                {
                    OrderItemID = oi.OrderItemID,
                    OrderID = oi.OrderID,
                    ItemID = oi.ItemID,
                    ItemName = oi.MenuItem.Name,
                    Quantity = oi.Quantity,
                    Subtotal = oi.Subtotal
                }).ToListAsync();

            return Ok(orderItems);
        }

        // GET: api/OrderItem/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItemDTO>> GetOrderItem(int id)
        {
            var orderItem = await _dbContext.OrderItems
                .Include(oi => oi.MenuItem)
                .Include(oi => oi.Order)
                .Where(oi => oi.OrderItemID == id)
                .Select(oi => new OrderItemDTO
                {
                    OrderItemID = oi.OrderItemID,
                    OrderID = oi.OrderID,
                    ItemID = oi.ItemID,
                    ItemName = oi.MenuItem.Name,
                    Quantity = oi.Quantity,
                    Subtotal = oi.Subtotal
                }).FirstOrDefaultAsync();

            if (orderItem == null)
            {
                return NotFound(new { Message = "Order item not found" });
            }

            return Ok(orderItem);
        }

        // POST: api/OrderItem
        [HttpPost]
        public async Task<ActionResult<OrderItemDTO>> CreateOrderItem([FromBody] OrderItemDTO orderItemDTO)
        {
            if (orderItemDTO == null || orderItemDTO.Quantity <= 0)
            {
                return BadRequest(new { Message = "Invalid order item data" });
            }

            var menuItem = await _dbContext.MenuItems.FindAsync(orderItemDTO.ItemID);
            if (menuItem == null)
            {
                return BadRequest(new { Message = "Invalid menu item ID" });
            }

            var order = await _dbContext.Orders.FindAsync(orderItemDTO.OrderID);
            if (order == null)
            {
                return BadRequest(new { Message = "Invalid order ID" });
            }

            var orderItem = new OrderItem
            {
                OrderID = orderItemDTO.OrderID,
                ItemID = orderItemDTO.ItemID,
                Quantity = orderItemDTO.Quantity,
                Subtotal = menuItem.Price * orderItemDTO.Quantity
            };

            _dbContext.OrderItems.Add(orderItem);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrderItem), new { id = orderItem.OrderItemID }, orderItemDTO);
        }

        // PUT: api/OrderItem/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderItem(int id, [FromBody] OrderItemDTO orderItemDTO)
        {
            var orderItem = await _dbContext.OrderItems.FindAsync(id);
            if (orderItem == null)
            {
                return NotFound(new { Message = "Order item not found" });
            }

            orderItem.Quantity = orderItemDTO.Quantity;
            orderItem.Subtotal = orderItemDTO.Subtotal;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { Message = "Concurrency conflict occurred while updating the order item" });
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
