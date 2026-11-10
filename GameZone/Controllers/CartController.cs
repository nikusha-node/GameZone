
using GameZone.DTOs.Cart;
using GameZone.DependencyInjection.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GameZone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        private int GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userId, out var id))
                throw new UnauthorizedAccessException("Invalid user token.");

            return id;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var cart = await _cartService.GetCart(GetUserId());

            return Ok(cart);
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddToCart(
            AddToCartRequest request)
        {
            var cart = await _cartService.AddToCart(
                GetUserId(), request);

            return Ok(cart);
        }

        [HttpPut("items/{id}")]
        public async Task<IActionResult> UpdateCartItem(
            int id,
            UpdateCartItemRequest request)
        {
            var cart = await _cartService.UpdateCartItem(
                GetUserId(), id, request);

            return Ok(cart);
        }

        [HttpDelete("items/{id}")]
        public async Task<IActionResult> RemoveCartItem(int id)
        {
            await _cartService.RemoveCartItem(GetUserId(), id);

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            await _cartService.ClearCart(GetUserId());

            return NoContent();
        }
    }
}
