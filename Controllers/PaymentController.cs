using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartCafeOrderingSystem_Api_V2.Context;
using SmartCafeOrderingSystem_Api_V2.DTOs;
using SmartCafeOrderingSystem_Api_V2.Models;

//Contains get all payments
// get a specific payment
// process a new payment simulated with 90-10 success/failure
// update payment status
// delete a payment

namespace SmartCafeOrderingSystem_Api_V2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public PaymentController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/Payment - Get all payments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentDTO>>> GetPayments()
        {
            var payments = await _dbContext.Payments.Include(p => p.Order)
                .Select(p => new PaymentDTO
                {
                    PaymentID = p.PaymentID,
                    OrderID = p.OrderID,
                    PaymentDate = p.PaymentDate,
                    Amount = p.Amount,
                    PaymentMethod = p.PaymentMethod,
                    PaymentStatus = p.PaymentStatus,
                    TransactionID = p.TransactionID ?? "",
                    GatewayResponse = p.GatewayResponse ?? ""
                }).ToListAsync();

            return Ok(payments);
        }

        // GET: api/Payment/{id} - Get a specific payment by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentDTO>> GetPayment(int id)
        {
            var payment = await _dbContext.Payments.Include(p => p.Order)
                .Where(p => p.PaymentID == id)
                .Select(p => new PaymentDTO
                {
                    PaymentID = p.PaymentID,
                    OrderID = p.OrderID,
                    PaymentDate = p.PaymentDate,
                    Amount = p.Amount,
                    PaymentMethod = p.PaymentMethod,
                    PaymentStatus = p.PaymentStatus,
                    TransactionID = p.TransactionID ?? "",
                    GatewayResponse = p.GatewayResponse ?? ""
                }).FirstOrDefaultAsync();

            if (payment == null)
            {
                return NotFound(new { Message = "Payment not found" });
            }

            return Ok(payment);
        }

        // POST: api/Payment - Process a simulated payment
        [HttpPost]
        public async Task<ActionResult<PaymentDTO>> ProcessMockPayment([FromBody] PaymentDTO paymentDTO)
        {
            if (paymentDTO == null || paymentDTO.Amount <= 0)
            {
                return BadRequest(new { Message = "Invalid payment data" });
            }

            var order = await _dbContext.Orders.FindAsync(paymentDTO.OrderID);
            if (order == null)
            {
                return BadRequest(new { Message = "Invalid order ID" });
            }

            if (order.TotalAmount != paymentDTO.Amount)
            {
                return BadRequest(new { Message = "Payment amount does not match order total" });
            }

            // Simulating a 90% success rate for the payment
            bool isPaymentSuccessful = new Random().Next(1, 11) <= 9; // 90% chance of success

            var payment = new Payment
            {
                OrderID = paymentDTO.OrderID,
                PaymentDate = DateTime.UtcNow,
                Amount = paymentDTO.Amount,
                PaymentMethod = paymentDTO.PaymentMethod,
                PaymentStatus = isPaymentSuccessful ? "Successful" : "Failed",
                TransactionID = $"TXN-{Guid.NewGuid()}",
                GatewayResponse = isPaymentSuccessful ? "Payment processed successfully" : "Payment failed due to insufficient balance"
            };

            if (isPaymentSuccessful)
            {
                order.PaymentStatus = "Paid";
            }

            _dbContext.Payments.Add(payment);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPayment), new { id = payment.PaymentID }, paymentDTO);
        }

        // PUT: api/Payment/{id}/status - Manually update payment status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdatePaymentStatus(int id, [FromBody] string status)
        {
            var payment = await _dbContext.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound(new { Message = "Payment not found" });
            }

            payment.PaymentStatus = status;
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Payment/{id} - Delete a payment record
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var payment = await _dbContext.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound(new { Message = "Payment not found" });
            }

            _dbContext.Payments.Remove(payment);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
