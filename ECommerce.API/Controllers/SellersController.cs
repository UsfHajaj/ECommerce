using AutoMapper;
using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Identity;
using ECommerce.Persistence.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IOrderService _service;

        public SellersController(UserManager<ApplicationUser> userManager,ApplicationDbContext context,IOrderService service)
        {
             _userManager = userManager;
            _context = context;
            _service = service;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterSellersDto sellersDto)
        {
            if (ModelState.IsValid)
            {


                var seller = new Sellers
                {
                    UserName = sellersDto.UserName,
                    Email = sellersDto.Email,
                    ImageUrl = sellersDto.ImageUrl,
                    Description = sellersDto.Description,

                };
                var result = await _userManager.CreateAsync(seller, sellersDto.Password);
                if (result.Succeeded)
                {
                    var productBrand = new ProductBrand
                    {
                        Name = sellersDto.BrandName, // تحتاج إلى إضافة خاصية BrandName في RegisterSellersDto
                        sellerId = seller.Id, // ربط الـ ProductBrand بالبائع
                        Sellers = seller // ربط الـ ProductBrand بالبائع
                    };
                    await _context.ProductBrands.AddAsync(productBrand);
                    await _context.SaveChangesAsync();

                    await _userManager.AddToRoleAsync(seller, "Seller");

                    return Ok(new { message = "Account created successfully with role." });
                }
                return BadRequest(result.Errors.FirstOrDefault()!.Description.ToString());
            }
            return BadRequest(ModelState);
        }
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var sellerId=User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (sellerId == null)
            {
                return NotFound("User not found");
            }

            

            var seller= await _context.Sellers
                .Include(m=>m.ProductBrand)
                .FirstOrDefaultAsync(s=>s.Id == sellerId);

            return Ok(new ShowSellerDto
            {
                Id = seller.Id,
                UserName = seller.UserName,
                Email = seller.Email,
                PhoneNumber = seller.PhoneNumber,
                ImageUrl = seller.ImageUrl,
                Description = seller.Description,
                brandId = seller.ProductBrand.Id
            });
            
        }

        [HttpPut]
        public async Task<IActionResult> PutProfile([FromBody] UpdateSellerProfileDto model)
        {
            var sellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (sellerId == null) return NotFound("User not found");
            var seller =await _context.Sellers
                .Include(m => m.ProductBrand)
                .FirstOrDefaultAsync(s => s.Id == sellerId);
            if (seller == null)
            {
                return NotFound("User not found");
            }
            await _context.SaveChangesAsync();

            seller.UserName = model.UserName ?? seller.UserName;
            seller.Email = model.Email ?? seller.Email;
            seller.PhoneNumber = model.PhoneNumber ?? seller.PhoneNumber;
            seller.ImageUrl = model.ImageUrl ?? seller.ImageUrl;
            seller.Description = model.Description ?? seller.Description;
            var result = await _userManager.UpdateAsync(seller);

            await _context.SaveChangesAsync();
            return Ok(new { message = "Profile updated successfully" });
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetSellerProducts()
        {
            var sellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (sellerId == null) return NotFound("User not found");

            var products = await _context.ProductBrands
                .Include(m => m.Products) // تأكد من تضمين المنتجات المرتبطة
                .ThenInclude(p => p.ProductBrand) // إذا كان هناك حاجة لربط المنتج بالبراند بشكل مباشر
                .Where(m => m.sellerId == sellerId)
                .SelectMany(m => m.Products)
                .ToListAsync();

            if (products == null || products.Count == 0)
            {
                return NotFound("No products found for this seller");
            }

            var productDtos = products.Select(p => new ProductDto
            {
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                ImageUrl = p.ImageUrl,
                // تحقق من أن ProductBrand موجود قبل الوصول إلى Id
                ProductBrandId = p.ProductBrand?.Id ?? 0, // 0 قيمة افتراضية إذا كانت null
                CategoryId = p.CategoryId // إذا كانت CategoryId يمكن أن تكون null، قد تحتاج لتعديل البيانات بحيث لا تكون null أو استخدام تحقق إضافي
            }).ToList();

            return Ok(productDtos);
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetOrders()
        {
            var sellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (sellerId == null) return NotFound("User not found");
            var orders =await _service.GetSellerOrdersAsync(sellerId);
            if (orders == null )
            {
                return NotFound("No orders found for this seller");
            }
            return Ok(orders);
        }


    }
}
