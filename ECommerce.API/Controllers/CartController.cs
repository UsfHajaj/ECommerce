using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities.Cart;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartServices _services;

        public CartController(ICartServices services)
        {
            _services = services;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _services.GetCartAsync(userId);
            if(cart == null)
            {
                return NotFound();
            }
            return Ok(cart);
        }
        [HttpPost("items")]
        public async Task<IActionResult> AddItemToCart([FromBody] AddCartItemDto item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            var addedItem = await _services.AddItemToCartAsync(userId, item);
            return CreatedAtAction(nameof(GetCart), new { id = addedItem.Id }, addedItem);
        }


        [HttpPut("items/{itemId}")]
        public async Task<IActionResult> UpdateCartItem(int id, [FromBody] UpdateCartItemDto item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != item.Id)
            {
                return BadRequest(new { message = "not is server" });
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var updatedItem = await _services.UpdateCartItemAsync(userId, item);
            if (userId == null)
            {
                return NotFound();
            }
            return Ok(updatedItem);
        }


        [HttpDelete("items/{itemId}")]
        public async Task<IActionResult> DeleteCartItem(int itemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _services.RemoveCartItemAsync(userId, itemId);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}
