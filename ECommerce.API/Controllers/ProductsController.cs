using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities;
using ECommerce.Persistence.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            return Ok( await _service.GetAllProductsAsync());
        }
        [HttpGet("getAllWithCategory")]
        public async Task<IActionResult> GetAllProductsWithCategory()
        {
            return Ok(await _service.GetProductsWithCategoryAsync());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _service.GetProductByIdWithCategoryAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] ProductDto productFromRequest)
        {
            if (productFromRequest == null)
            {
                return BadRequest("Product data is null");
            }
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = new Product
            {
                Name = productFromRequest.Name,
                Description = productFromRequest.Description,
                Price = productFromRequest.Price,
                StockQuantity = productFromRequest.StockQuantity,
                ImageUrl = productFromRequest.ImageUrl,
                CategoryId = productFromRequest.CategoryId,
                ProductBrandId = productFromRequest.ProductBrandId
            };
            await _service.AddProductAsync(product);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, [FromBody] ProductDto productFromRequest)
        {
            var product = await _service.GetProductByIdAsync(id);

            if (product == null || product.Id != id)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            product.Name = productFromRequest.Name;
            product.Description = productFromRequest.Description;
            product.Price = productFromRequest.Price;
            product.StockQuantity = productFromRequest.StockQuantity;
            product.ImageUrl = productFromRequest.ImageUrl;
            product.CategoryId = productFromRequest.CategoryId;
            product.ProductBrandId = productFromRequest.ProductBrandId;

            await _service.UpdateProductAsync(product);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product= await _service.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            await _service.DeleteProductAsync(product);
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetSearchProduct([FromQuery] string query)
        {
            var products = await _service
                .SearchProductsAsync(query);
            return Ok(products);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetProductcategory(int categoryId)
        {
            var products =await _service.GetProductsByCategoryAsync(categoryId);
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }
    }
}
