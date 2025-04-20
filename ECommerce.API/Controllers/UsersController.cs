using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities.Identity;
using ECommerce.Persistence.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UsersController(UserManager<ApplicationUser> userManager,ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return NotFound("User not found");
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }
            return Ok(new
            {
                user.Id,
                user.UserName,
                user.Email,
                user.Address,
                user.PhoneNumber
            });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDto model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return NotFound("User not found");
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }
            user.UserName = model.UserName ?? user.UserName;
            user.Email = model.Email ?? user.Email;
            user.PhoneNumber = model.PhoneNumber ?? user.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest("Failed to update profile");
            }
            return Ok(new
            {
                user.Id,
                user.UserName,
                user.Email,
                user.PhoneNumber
            });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAccount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return NotFound("User not found");
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok("Account deleted successfully");
            }
            return BadRequest("Failed to delete account");
        }

        [HttpGet("address")]
        public async Task<IActionResult> GetUserAddress()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userId == null) return NotFound("User not found");
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var address = await _context.Addresses.FirstOrDefaultAsync(m => m.UserId == userId);
            if (address == null)
                return NotFound();
            var addressDto = new AddressDto
            {
                Street = address.Street,
                City = address.City,
                State = address.State,
                Country = address.Country,
                Zipcode = address.Zipcode
            };


            return Ok(addressDto);
        }

        [HttpPost("address")]
        public async Task<IActionResult> PostUserAddress([FromBody] AddressDto addressDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return NotFound("User not found");

            var existing = await _context.Addresses.FirstOrDefaultAsync(a => a.UserId == userId);
            if (existing != null)
                return BadRequest("User already has an address");

            var adderss = new Address();
            adderss.UserId= userId;
            adderss.Street = addressDto.Street;
            adderss.City= addressDto.City;
            adderss.State= addressDto.State;
            adderss.Zipcode= addressDto.Zipcode;

            await _context.Addresses.AddAsync(adderss);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUserAddress), new { id = adderss.Id }, adderss);
        }

        [HttpPut("address")]
        public async Task<IActionResult> PutUserAddress([FromBody] AddressDto addressDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return NotFound("User not found");

            var address = await _context.Addresses.FirstOrDefaultAsync(m => m.UserId == userId);
            if (address == null) return BadRequest();

            address.State = addressDto.State;
            address.Street = addressDto.Street;
            address.City= addressDto.City;
            address.Country= addressDto.Country;
            address.Zipcode= addressDto.Zipcode;

            _context.Addresses.Update(address);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("address")]
        public async Task<IActionResult> DeleteAddress()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return NotFound("User not found");

            var address =await _context.Addresses.FirstOrDefaultAsync(m=>m.UserId == userId);
            if (address == null) return BadRequest();

            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
