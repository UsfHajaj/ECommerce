using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _service;

        public PaymentsController(IPaymentService service)
        {
            _service = service;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentDto>> GetPaymentById(int id)
        {
            var userId=User.FindFirstValue(ClaimTypes.NameIdentifier);
            var payment = await _service.GetPaymentByIdAsync(id, userId);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }
        [HttpPost("process")]
        public async Task<ActionResult<PaymentDto>> ProcessPayment([FromBody] PaymentProcessDto paymentProcessDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var payment = await _service.ProcessPaymentAsync(userId, paymentProcessDto);
            if (payment == null)
            {
                return BadRequest("Payment processing failed.");
            }
            return CreatedAtAction(nameof(GetPaymentById), new {id=payment.Id},payment);
        }

        [HttpPost("refund/{id}")]
        public async Task<ActionResult<PaymentDto>> RefundPayment(int id, [FromBody] PaymentRefundDto refundDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var payment =await _service.RefundPaymentAsync(id, refundDto, userId);
            if (payment == null)
            {
                return BadRequest("Refund processing failed.");
            }
            return Ok(payment);
        }
    }
}
