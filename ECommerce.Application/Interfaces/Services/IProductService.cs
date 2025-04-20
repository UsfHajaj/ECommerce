using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(Product product);
        Task<IEnumerable<ProductCategoryDto>> SearchProductsAsync(string searchTerm);
        Task<IEnumerable<ProductCategoryDto>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<ProductCategoryDto>> GetProductsWithCategoryAsync();
        Task<ProductCategoryDto> GetProductByIdWithCategoryAsync(int id);
    }
}
