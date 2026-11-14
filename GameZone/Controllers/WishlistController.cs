using GameZone.DependencyInjection.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GameZone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        private int GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userId, out var id))
                throw new UnauthorizedAccessException("Invalid user token.");

            return id;
        }

        [HttpGet]
        public async Task<IActionResult> GetWishlist()
        {
            var wishlist = await _wishlistService.GetWishlist(GetUserId());
            return Ok(wishlist);
        }

        [HttpPost("items/{productId:int}")]
        public async Task<IActionResult> AddProduct(int productId)
        {
            var wishlist = await _wishlistService.AddProduct(GetUserId(), productId);
            return Ok(wishlist);
        }

        [HttpDelete("items/{productId:int}")]
        public async Task<IActionResult> RemoveProduct(int productId)
        {
            var result = await _wishlistService.RemoveProduct(GetUserId(), productId);

            if (!result)
                return NotFound(new { Message = "Wishlist item not found." });

            return NoContent();
        }

        [HttpDelete("items")]
        public async Task<IActionResult> ClearWishlist()
        {
            var result = await _wishlistService.ClearWishlist(GetUserId());

            if (!result)
                return NotFound(new { Message = "Wishlist not found." });

            return NoContent();
        }
    }
}
