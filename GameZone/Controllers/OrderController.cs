
using GameZone.DependencyInjection.Interfaces;
using GameZone.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GameZone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        private int GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userId, out var id))
                throw new UnauthorizedAccessException("Invalid user token.");

            return id;
        }

        [HttpPost("checkout")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Checkout()
        {
            var result = await _orderService.Checkout(GetUserId());
            return Ok(result);
        }

        [HttpGet("my-orders")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetMyOrders()
        {
            var orders = await _orderService.GetUserOrders(GetUserId());
            return Ok(orders);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderById(GetUserId(), id);

            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrders();
            return Ok(orders);
        }

        [HttpGet("admin/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOrderDetails(int id)
        {
            var order = await _orderService.GetOrderById(id);

            if (order == null)
                return NotFound(new { Message = "Order not found." });

            return Ok(order);
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrderStatus(
            int id, OrderStatus status)
        {
            await _orderService.UpdateOrderStatus(id, status);
            return NoContent();
        }
    }
}
