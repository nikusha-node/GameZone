using GameZone.Api.DTOs;
using GameZone.Api.Helpers;
using GameZone.Api.Interfaces;
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
}