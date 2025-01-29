using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartCafeOrderingSystem_Api_V2.Context;
using SmartCafeOrderingSystem_Api_V2.Models;

//Contains get all payments
// get a specific payment
// process a new payment
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

        // GET: api/Payment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPayments()
        {
            return await _dbContext.Payments.Include(p => p.Order).ToListAsync();
        }

        // GET: api/Payment/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetPayment(int id)
        {
            var payment = await _dbContext.Payments.Include(p => p.Order)
                                                   .FirstOrDefaultAsync(p => p.PaymentID == id);

            if (payment == null)
            {
                return NotFound(new { Message = "Payment not found" });
            }

            return payment;
        }

        // POST: api/Payment
        [HttpPost]
        public async Task<ActionResult<Payment>> ProcessPayment(Payment payment)
        {
            if (payment == null || payment.Amount <= 0)
            {
                return BadRequest(new { Message = "Invalid payment data" });
            }

            var order = await _dbContext.Orders.FindAsync(payment.OrderID);
            if (order == null)
            {
                return BadRequest(new { Message = "Invalid order ID" });
            }

            if (order.TotalAmount != payment.Amount)
            {
                return BadRequest(new { Message = "Payment amount does not match order total" });
            }

            payment.PaymentStatus = "Successful"; // Assume payment is successful
            order.PaymentStatus = "Paid"; // Update order payment status

            _dbContext.Payments.Add(payment);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPayment), new { id = payment.PaymentID }, payment);
        }

        // PUT: api/Payment/{id}/status
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

        // DELETE: api/Payment/{id}
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
