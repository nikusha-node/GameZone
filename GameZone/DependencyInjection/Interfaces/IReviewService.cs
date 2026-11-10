using GameZone.DTOs.Review;

namespace GameZone.DependencyInjection.Interfaces
{
    public interface IReviewService
    {
        Task<ReviewResponse> Create(int userId, CreateReviewRequest request);
        Task<List<ReviewResponse>> GetProductReviews(int productId);
        Task<List<ReviewResponse>> GetUserReviews(int userId);
        Task<bool> Update(int userId, int reviewId, UpdateReviewRequest request);
        Task<bool> Delete(int userId, int reviewId);
        Task<bool> AdminDelete(int reviewId);
    }
}