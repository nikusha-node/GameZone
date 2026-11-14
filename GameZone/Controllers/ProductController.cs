using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GameZone.DTOs.Product;
using GameZone.DependencyInjection.Interfaces;

namespace GameZone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAll();
            return Ok(products);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetFiltered(
    [FromQuery] ProductQueryParameters parameters)
        {
            if (parameters.MinPrice < 0 || parameters.MaxPrice < 0)
                return BadRequest("Prices cannot be negative.");

            if (parameters.MinPrice.HasValue &&
                parameters.MaxPrice.HasValue &&
                parameters.MinPrice > parameters.MaxPrice)
                return BadRequest("Minimum price cannot exceed maximum price.");

            var products = await _productService.GetFiltered(parameters);

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetById(id);

            if (product == null)
                return NotFound("Product not found.");

            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateProductRequest request)
        {
            var product = await _productService.Create(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = product.Id },
                product);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(
            int id,
            UpdateProductRequest request)
        {
            var result = await _productService.Update(id, request);

            if (!result)
                return NotFound("Product not found.");

            return Ok("Product updated successfully.");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.Delete(id);

            if (!result)
                return NotFound("Product not found.");

            return Ok("Product deleted successfully.");
        }
    }
}