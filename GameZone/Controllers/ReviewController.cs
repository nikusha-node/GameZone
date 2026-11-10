
using GameZone.DTOs.Review;
using GameZone.DependencyInjection.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GameZone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        private int GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userId, out var id))
                throw new UnauthorizedAccessException("Invalid user token.");

            return id;
        }

        [HttpGet("product/{productId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductReviews(int productId)
        {
            var reviews = await _reviewService.GetProductReviews(productId);
            return Ok(reviews);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Create(CreateReviewRequest request)
        {
            var review = await _reviewService.Create(GetUserId(), request);
            return Ok(review);
        }

        [HttpGet("my-reviews")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetMyReviews()
        {
            var reviews = await _reviewService.GetUserReviews(GetUserId());
            return Ok(reviews);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Update(
            int id, UpdateReviewRequest request)
        {
            await _reviewService.Update(GetUserId(), id, request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Delete(int id)
        {
            await _reviewService.Delete(GetUserId(), id);
            return NoContent();
        }

        [HttpDelete("admin/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminDelete(int id)
        {
            await _reviewService.AdminDelete(id);
            return NoContent();
        }
    }
}
