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
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _repository;

        public ReviewService(IReviewRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<ReviewToShowDto>> GetAllReviewsAsync()
        {
            return await _repository.AllReviews();
        }

        public async Task<List<ReviewToShowDto>> GetReviewsWithProductIdAsync(int productId)
        {
            return await _repository.ReviewsWithProductId(productId);
        }

        public async Task<List<ReviewToShowDto>> GetReviewWithUserIDAsync(string userId)
        {
            return await _repository.ReviewWithUserID(userId);
        }

        public async Task PostAddReviewsAsync(Review review)
        {
            await _repository.AddReviews(review);
            await _repository.SaveAsync();
        }
    }
}
