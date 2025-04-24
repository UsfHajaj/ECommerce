using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces.Services
{
    public interface IReviewService
    {
        Task<List<ReviewToShowDto>> GetAllReviewsAsync();
        Task<List<ReviewToShowDto>> GetReviewWithUserIDAsync(string userId);
        Task<List<ReviewToShowDto>> GetReviewsWithProductIdAsync(int productId);
        Task PostAddReviewsAsync(Review review);
    }
}
