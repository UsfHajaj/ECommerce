using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Identity;
using ECommerce.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Repositories
{
    public class ReviewRepository :GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<List<ReviewToShowDto>> ReviewsWithProductId(int productId)
        {
            var reviews=await _dbSet
                .Include(m=>m.User)
                .Include(p=>p.Product)
                .Where(m=>m.ProductId == productId)
                .Select(m => new ReviewToShowDto
                {
                    Id = m.Id,
                    ProductId = m.ProductId,
                    Rating = m.Rating,
                    Comment = m.Comment,
                    UserName = m.User.UserName,
                    ProductName = m.Product.Name
                })
                .ToListAsync();
            return reviews;
        }

        public async Task<List<ReviewToShowDto>> ReviewWithUserID(string userId)
        {
            var review= await _dbSet
                .Include(m=>m.User)
                .Include(m => m.Product)
                .Where(m => m.UserId == userId)
                .Select(m => new ReviewToShowDto
                {
                    Id = m.Id,
                    ProductId = m.ProductId,
                    Rating = m.Rating,
                    Comment = m.Comment,
                    UserName=m.User.UserName,
                    ProductName = m.Product.Name
                })
                .ToListAsync();
            return review;
        }

        public async Task AddReviews(Review review)
        {
            var entity =await _dbSet.AddAsync(review);
        }

        public async Task<List<ReviewToShowDto>> AllReviews()
        {
            return await _dbSet
                .Include(m => m.User)
                .Include(m=>m.Product)
                .Select(m => new ReviewToShowDto
                {
                    Id = m.Id,
                    ProductId = m.ProductId,
                    Rating = m.Rating,
                    Comment = m.Comment,
                    UserName = m.User.UserName,
                    ProductName = m.Product.Name
                })
                .ToListAsync();
        }
    }
}
