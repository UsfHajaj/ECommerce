using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces.Repositories
{
    public interface IProductRepository:IGenericRepository<Product>
    {
        Task<List<ProductCategoryDto>> FindAsync(Expression<Func<Product, bool>> predicate);
        Task<IEnumerable<ProductCategoryDto>> ProductsWithCategoryAsync();
        Task<ProductCategoryDto> ProductByIdWithCategoryAsync(int id);
    }
}
