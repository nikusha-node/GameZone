
using GameZone.Data;
using GameZone.DTOs.Review;
using GameZone.DependencyInjection.Interfaces;
using GameZone.Exceptions;
using GameZone.Models;
using Microsoft.EntityFrameworkCore;

namespace GameZone.DependencyInjection.Services
{
    public class ReviewService : IReviewService
    {
        private readonly DataContext _context;

        public ReviewService(DataContext context)
        {
            _context = context;
        }

        public async Task<ReviewResponse> Create(
            int userId, CreateReviewRequest request)
        {
            var productExists = await _context.Products
                .AnyAsync(p => p.Id == request.ProductId);

            if (!productExists)
                throw new NotFoundException("Product not found.");

            var alreadyReviewed = await _context.Reviews
                .AnyAsync(r => r.UserId == userId &&
                               r.ProductId == request.ProductId);

            if (alreadyReviewed)
                throw new ConflictException(
                    "You have already reviewed this product.");

            var review = new Review
            {
                UserId = userId,
                ProductId = request.ProductId,
                Rating = request.Rating,
                Comment = request.Comment,
                CreatedAt = DateTime.UtcNow
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            await _context.Entry(review)
                .Reference(r => r.User)
                .LoadAsync();

            return MapToResponse(review);
        }

        public async Task<List<ReviewResponse>> GetProductReviews(
            int productId)
        {
            var productExists = await _context.Products
                .AnyAsync(p => p.Id == productId);

            if (!productExists)
                throw new NotFoundException("Product not found.");

            var reviews = await _context.Reviews
                .Include(r => r.User)
                .Where(r => r.ProductId == productId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return reviews.Select(MapToResponse).ToList();
        }

        public async Task<List<ReviewResponse>> GetUserReviews(
            int userId)
        {
            var reviews = await _context.Reviews
                .Include(r => r.User)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return reviews.Select(MapToResponse).ToList();
        }

        public async Task<bool> Update(
            int userId, int reviewId, UpdateReviewRequest request)
        {
            var review = await _context.Reviews
                .FirstOrDefaultAsync(r =>
                    r.Id == reviewId && r.UserId == userId);

            if (review == null)
                throw new NotFoundException("Review not found.");

            review.Rating = request.Rating;
            review.Comment = request.Comment;
            review.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(int userId, int reviewId)
        {
            var review = await _context.Reviews
                .FirstOrDefaultAsync(r =>
                    r.Id == reviewId && r.UserId == userId);

            if (review == null)
                throw new NotFoundException("Review not found.");

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AdminDelete(int reviewId)
        {
            var review = await _context.Reviews
                .FirstOrDefaultAsync(r => r.Id == reviewId);

            if (review == null)
                throw new NotFoundException("Review not found.");

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return true;
        }

        private static ReviewResponse MapToResponse(Review review)
        {
            return new ReviewResponse
            {
                Id = review.Id,
                UserId = review.UserId,
                UserName = $"{review.User.FirstName} {review.User.LastName}",
                ProductId = review.ProductId,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt
            };
        }
    }
}
