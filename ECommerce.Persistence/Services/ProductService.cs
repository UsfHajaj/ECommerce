using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
            => await _repository.GetAllAsync();

        public async Task<Product> GetProductByIdAsync(int id)
            => await _repository.GetByIdAsync(id);
        public async Task AddProductAsync(Product product)
        {
            await _repository.AddAsync(product);
            await _repository.SaveAsync();
        }
        public async Task UpdateProductAsync(Product product)
        {
            await _repository.UpdateAsync(product);
            await _repository.SaveAsync();
        }
        public async Task DeleteProductAsync(Product product)
        {
              await _repository.DeleteAsync(product);
              await _repository.SaveAsync();
        }

        public async Task<IEnumerable<ProductCategoryDto>> GetProductsByCategoryAsync(int categoryId)
        {
            var products = await _repository
                .FindAsync(p => p.CategoryId == categoryId);
            return products ?? Enumerable.Empty<ProductCategoryDto>();
        }

        public async Task<IEnumerable<ProductCategoryDto>> SearchProductsAsync(string searchTerm)
        {
            var products = await _repository
                .FindAsync(p => p.Name.Contains(searchTerm) 
                || p.Description.Contains(searchTerm));

            return products ?? Enumerable.Empty<ProductCategoryDto>();
        }

        public async Task<IEnumerable<ProductCategoryDto>> GetProductsWithCategoryAsync()
        {
           return await _repository.ProductsWithCategoryAsync();
        }

        public Task<ProductCategoryDto> GetProductByIdWithCategoryAsync(int id)
        {
            return _repository.ProductByIdWithCategoryAsync(id);
        }
    }
}
