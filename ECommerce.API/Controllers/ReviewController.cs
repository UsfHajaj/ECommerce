using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _service;

        public ReviewController(IReviewService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewToShowDto>>> GetAllReviews()
        {
            return await _service.GetAllReviewsAsync();
        }
        [HttpGet("product/{productId}")]
        public async Task<ActionResult<IEnumerable<ReviewToShowDto>>> GetReviewsWithProductId(int productId)
        {
            return await _service.GetReviewsWithProductIdAsync(productId);
        }
        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<ReviewToShowDto>>> GetReviewWithUserID()
        {
            var userId=User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _service.GetReviewWithUserIDAsync(userId);
        }
        [HttpPost]
        public async Task<IActionResult> PostAddReviews([FromBody] ReviewDto reviewFromDto)
        {
            var user=User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (reviewFromDto == null)
            {
                return BadRequest("Review cannot be null");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var review = new Review
            {
                ProductId = reviewFromDto.ProductId,
                Rating = reviewFromDto.Rating,
                Comment = reviewFromDto.Comment,
                UserId = user
            };

            await _service.PostAddReviewsAsync(review);
            return CreatedAtAction(nameof(GetAllReviews), new { id = review.Id }, review);
        }
    }
}
