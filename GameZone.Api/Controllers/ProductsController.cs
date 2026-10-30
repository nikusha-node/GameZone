using GameZone.Api.Configurations;
using GameZone.Api.DTOs;
using GameZone.Api.Helpers;
using GameZone.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameZone.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    // ---------- Public ----------

    [HttpGet]
    public async Task<ActionResult<PagedResult<ProductListDto>>> GetAll(
        [FromQuery] ProductQueryParams query, CancellationToken ct)
        => Ok(await _productService.GetPagedAsync(query, ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductResponseDto>> GetById(int id, CancellationToken ct)
        => Ok(await _productService.GetByIdAsync(id, ct));

    [HttpGet("by-slug/{slug}")]
    public async Task<ActionResult<ProductResponseDto>> GetBySlug(string slug, CancellationToken ct)
        => Ok(await _productService.GetBySlugAsync(slug, ct));

    // ---------- Admin ----------

    [Authorize(Roles = AppRoles.Admin)]
    [HttpGet("admin")]
    public async Task<ActionResult<PagedResult<ProductListDto>>> GetAllForAdmin(
        [FromQuery] ProductQueryParams query, CancellationToken ct)
        => Ok(await _productService.GetPagedForAdminAsync(query, ct));

    [Authorize(Roles = AppRoles.Admin)]
    [HttpPost]
    public async Task<ActionResult<ProductResponseDto>> Create(CreateProductDto dto, CancellationToken ct)
    {
        var created = await _productService.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [Authorize(Roles = AppRoles.Admin)]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProductResponseDto>> Update(int id, UpdateProductDto dto, CancellationToken ct)
        => Ok(await _productService.UpdateAsync(id, dto, ct));

    [Authorize(Roles = AppRoles.Admin)]
    [HttpPatch("{id:int}/stock")]
    public async Task<IActionResult> UpdateStock(int id, UpdateStockDto dto, CancellationToken ct)
    {
        await _productService.UpdateStockAsync(id, dto, ct);
        return NoContent();
    }

    [Authorize(Roles = AppRoles.Admin)]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _productService.DeleteAsync(id, ct);
        return NoContent();
    }

    [Authorize(Roles = AppRoles.Admin)]
    [HttpPost("{id:int}/images")]
    public async Task<ActionResult<ProductResponseDto>> AddImage(int id, ProductImageInputDto dto, CancellationToken ct)
        => Ok(await _productService.AddImageAsync(id, dto, ct));

    [Authorize(Roles = AppRoles.Admin)]
    [HttpDelete("{id:int}/images/{imageId:int}")]
    public async Task<IActionResult> RemoveImage(int id, int imageId, CancellationToken ct)
    {
        await _productService.RemoveImageAsync(id, imageId, ct);
        return NoContent();
    }

    [Authorize(Roles = AppRoles.Admin)]
    [HttpPost("{id:int}/specifications")]
    public async Task<ActionResult<ProductResponseDto>> AddSpecification(int id, ProductSpecificationInputDto dto, CancellationToken ct)
        => Ok(await _productService.AddSpecificationAsync(id, dto, ct));

    [Authorize(Roles = AppRoles.Admin)]
    [HttpDelete("{id:int}/specifications/{specId:int}")]
    public async Task<IActionResult> RemoveSpecification(int id, int specId, CancellationToken ct)
    {
        await _productService.RemoveSpecificationAsync(id, specId, ct);
        return NoContent();
    }
}