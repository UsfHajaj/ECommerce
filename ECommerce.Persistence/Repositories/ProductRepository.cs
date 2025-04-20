using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Domain.Entities;
using ECommerce.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<List<ProductCategoryDto>> FindAsync(Expression<Func<Product, bool>> predicate)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Include(p => p.ProductBrand)
                .Where(predicate)
                .Select(m => new ProductCategoryDto
                {
                    Name = m.Name,
                    Description = m.Description,
                    Price = m.Price,
                    StockQuantity = m.StockQuantity,
                    ImageUrl = m.ImageUrl,
                    CategoryName = m.Category.Name,
                    ProductBrandName = m.ProductBrand.Name
                })
                .ToListAsync();
        }
        public async Task<IEnumerable<ProductCategoryDto>> ProductsWithCategoryAsync()
        {
           var product= await _dbSet
                .Include(p => p.Category)
                .Include(p => p.ProductBrand)
                .Select(m => new ProductCategoryDto
                {
                    Name = m.Name,
                    Description = m.Description,
                    Price = m.Price,
                    StockQuantity = m.StockQuantity,
                    ImageUrl = m.ImageUrl,
                    CategoryName = m.Category.Name,
                    ProductBrandName = m.ProductBrand.Name
                } ).ToListAsync();
            return product;
        }

        public Task<ProductCategoryDto> ProductByIdWithCategoryAsync(int id)
        {
            var product= _dbSet
                .Include(p => p.Category)
                .Include(p => p.ProductBrand)
                .Where(m => m.Id == id)
                .Select(m => new ProductCategoryDto
                {
                    Name = m.Name,
                    Description = m.Description,
                    Price = m.Price,
                    StockQuantity = m.StockQuantity,
                    ImageUrl = m.ImageUrl,
                    CategoryName = m.Category.Name,
                    ProductBrandName = m.ProductBrand.Name
                }).FirstOrDefaultAsync();
            return product!;
        }
    }
}
