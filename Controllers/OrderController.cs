﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartCafeOrderingSystem_Api_V2.Context;
using SmartCafeOrderingSystem_Api_V2.DTOs;
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
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrders()
        {
            var orders = await _dbContext.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .Select(o => new OrderDTO
                {
                    OrderID = o.OrderID,
                    UserID = o.UserID,
                    OrderDate = o.OrderDate,
                    OrderType = o.OrderType,
                    TableNumber = o.TableNumber,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status,
                    PaymentStatus = o.PaymentStatus
                }).ToListAsync();

            return Ok(orders);
        }

        // GET: api/Order/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDTO>> GetOrder(int id)
        {
            var order = await _dbContext.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .Where(o => o.OrderID == id)
                .Select(o => new OrderDTO
                {
                    OrderID = o.OrderID,
                    UserID = o.UserID,
                    OrderDate = o.OrderDate,
                    OrderType = o.OrderType,
                    TableNumber = o.TableNumber,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status,
                    PaymentStatus = o.PaymentStatus
                }).FirstOrDefaultAsync();

            if (order == null)
            {
                return NotFound(new { Message = "Order not found" });
            }

            return Ok(order);
        }

        // GET: api/Order/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetUserOrders(int userId)
        {
            var orders = await _dbContext.Orders
                .Where(o => o.UserID == userId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .Select(o => new OrderDTO
                {
                    OrderID = o.OrderID,
                    UserID = o.UserID,
                    OrderDate = o.OrderDate,
                    OrderType = o.OrderType,
                    TableNumber = o.TableNumber,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status,
                    PaymentStatus = o.PaymentStatus
                }).ToListAsync();

            if (!orders.Any())
            {
                return NotFound(new { Message = "No orders found for this user" });
            }

            return Ok(orders);
        }

        // POST: api/Order
        [HttpPost]
        public async Task<ActionResult<OrderDTO>> CreateOrder([FromBody] OrderDTO orderDTO)
        {
            if (orderDTO == null)
            {
                return BadRequest(new { Message = "Invalid order data" });
            }

            var order = new Order
            {
                UserID = orderDTO.UserID,
                OrderDate = DateTime.UtcNow,
                OrderType = orderDTO.OrderType,
                TableNumber = orderDTO.TableNumber,
                TotalAmount = orderDTO.TotalAmount,
                Status = "Placed",
                PaymentStatus = "Pending"
            };

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrder), new { id = order.OrderID }, orderDTO);
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
