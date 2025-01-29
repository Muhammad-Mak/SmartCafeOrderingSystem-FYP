using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartCafeOrderingSystem_Api_V2.Context;
using SmartCafeOrderingSystem_Api_V2.Models;

// contains get all orders
// get a specific order
// get orders by a specific user
// create a new order
// update order status
// cancel an order

namespace SmartCafeOrderingSystem_Api_V2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public OrderController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/Order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _dbContext.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .ToListAsync();
        }

        // GET: api/Order/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _dbContext.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .FirstOrDefaultAsync(o => o.OrderID == id);

            if (order == null)
            {
                return NotFound(new { Message = "Order not found" });
            }

            return order;
        }

        // GET: api/Order/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetUserOrders(int userId)
        {
            var orders = await _dbContext.Orders
                .Where(o => o.UserID == userId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .ToListAsync();

            if (!orders.Any())
            {
                return NotFound(new { Message = "No orders found for this user" });
            }

            return orders;
        }

        // POST: api/Order
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(Order order)
        {
            if (order == null || order.OrderItems == null || !order.OrderItems.Any())
            {
                return BadRequest(new { Message = "Invalid order data" });
            }

            // Check if user exists
            var userExists = await _dbContext.Users.AnyAsync(u => u.UserID == order.UserID);
            if (!userExists)
            {
                return BadRequest(new { Message = "Invalid user ID" });
            }

            // Calculate total amount
            decimal totalAmount = 0;
            foreach (var item in order.OrderItems)
            {
                var menuItem = await _dbContext.MenuItems.FindAsync(item.ItemID);
                if (menuItem == null)
                {
                    return BadRequest(new { Message = $"MenuItem with ID {item.ItemID} not found" });
                }
                item.Subtotal = menuItem.Price * item.Quantity;
                totalAmount += item.Subtotal;
            }

            order.TotalAmount = totalAmount;
            order.Status = "Placed"; // Default status
            order.PaymentStatus = "Pending";

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrder), new { id = order.OrderID }, order);
        }

        // PUT: api/Order/{id}/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] string status)
        {
            var order = await _dbContext.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound(new { Message = "Order not found" });
            }

            order.Status = status;
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Order/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var order = await _dbContext.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound(new { Message = "Order not found" });
            }

            _dbContext.Orders.Remove(order);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
