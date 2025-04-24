using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces.Repositories
{
    public interface IReviewRepository :IGenericRepository<Review>
    {
        Task<List<ReviewToShowDto>> AllReviews();
        Task<List<ReviewToShowDto>> ReviewWithUserID(string userId);
        Task<List<ReviewToShowDto>> ReviewsWithProductId(int productId);
        Task AddReviews(Review review);
    }
}
