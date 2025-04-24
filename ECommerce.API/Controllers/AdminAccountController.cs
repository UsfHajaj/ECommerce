using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities.Identity;
using ECommerce.Persistence.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce.API.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class AdminAccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AdminAccountController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO adminDto)
        {
            if (ModelState.IsValid)
            {
                var admin = new ApplicationUser
                {
                    UserName = adminDto.UserName,
                    Email = adminDto.Email,
                };
                var result = await _userManager.CreateAsync(admin, adminDto.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(admin, "Admin");
                    return Ok(new { message = "Account created successfully with role." });
                }
                return BadRequest(result.Errors.FirstOrDefault()!.Description.ToString());


            }
            return BadRequest(ModelState);
        }
        [HttpGet("users")]
        public IActionResult GetAllUser()
        {
            var allusers = _userManager.Users.ToList();
            var users = new List<ApplicationUser>();
            foreach (var user in allusers)
            {
                var roles = _userManager.GetRolesAsync(user).Result;
                if (roles.Contains("User"))
                {
                    users.Add(user);
                }
            }
            if (!users.Any())
            {
                return NotFound("No users found");
            }
            return Ok(users);
        }
        [HttpGet("orders")]
        public IActionResult GetAllOrders()
        {
            var orders = _context.Orders.ToList();
            if (orders == null)
            {
                return NotFound("No orders found");
            }
            return Ok(orders);
        }
        [HttpGet("product")]
        public IActionResult GetProducta()
        {
            var products = _context.Products.ToList();
            if (products == null)
            {
                return NotFound("No products found");
            }
            return Ok(products);
        }
        [HttpGet("sellers")]
        public async Task<IActionResult> GetSellers()
        {
            var allUser = _userManager.Users.ToList();
            var sellers =new List<ApplicationUser>();
            foreach (var user in allUser)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("Seller"))
                {
                    sellers.Add(user);
                }
            }
            if (!sellers.Any())
            {
                return NotFound("No sellers found");
            }
            return Ok(sellers);
        }
    }
}
