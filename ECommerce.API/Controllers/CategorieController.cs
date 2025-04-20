using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategorieController : ControllerBase
    {
        private readonly ICategoriesService _service;

        public CategorieController(ICategoriesService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            return Ok(await _service.GetAllCategoriesAsync());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategorieById(int id)
        {
            var category = await _service.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }
        [HttpPost]
        public async Task<IActionResult> PostCategorie([FromBody] CategoryDto categoryFromRequest)
        {
            if (categoryFromRequest == null)
            {
                return NotFound("Category data is null");
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var category = new Category
            {
                Name = categoryFromRequest.Name,
                Description = categoryFromRequest.Description
            };
            await _service.CreateCategoryAsync(category);
            return CreatedAtAction(nameof(GetCategorieById), new { id = category.Id }, category);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategorie(int id, [FromBody] CategoryDto categoryFromRequest)
        {
            var category = await _service.GetCategoryByIdAsync(id);

            if (category == null)
            {
                return NotFound("Category data is null");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            category.Name = categoryFromRequest.Name;
            category.Description = categoryFromRequest.Description;

            await _service.UpdateCategoryAsync(category);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategorie(int id)
        {
            var category = await _service.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            await _service.DeleteCategoryAsync(category);
            return NoContent();
        }
    }
}
