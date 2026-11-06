using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GameZone.DTOs.Brand;
using GameZone.DependencyInjection.Interfaces;

namespace GameZone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var brands = await _brandService.GetAll();
            return Ok(brands);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var brand = await _brandService.GetById(id);

            if (brand == null)
                return NotFound("Brand not found.");

            return Ok(brand);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateBrandRequest request)
        {
            var brand = await _brandService.Create(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = brand.Id },
                brand);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(
            int id,
            UpdateBrandRequest request)
        {
            var result = await _brandService.Update(id, request);

            if (!result)
                return NotFound("Brand not found.");

            return Ok("Brand updated successfully.");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _brandService.Delete(id);

            if (!result)
                return NotFound("Brand not found.");

            return Ok("Brand deleted successfully.");
        }
    }
}