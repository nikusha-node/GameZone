using GameZone.Api.Configurations;
using GameZone.Api.DTOs;
using GameZone.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameZone.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BrandsController : ControllerBase
{
    private readonly IBrandService _brandService;

    public BrandsController(IBrandService brandService)
    {
        _brandService = brandService;
    }

    [HttpGet]
    public async Task<ActionResult<List<BrandDto>>> GetAll(CancellationToken ct)
        => Ok(await _brandService.GetAllAsync(ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BrandDto>> GetById(int id, CancellationToken ct)
        => Ok(await _brandService.GetByIdAsync(id, ct));

    [Authorize(Roles = AppRoles.Admin)]
    [HttpPost]
    public async Task<ActionResult<BrandDto>> Create(CreateBrandDto dto, CancellationToken ct)
    {
        var created = await _brandService.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [Authorize(Roles = AppRoles.Admin)]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<BrandDto>> Update(int id, UpdateBrandDto dto, CancellationToken ct)
        => Ok(await _brandService.UpdateAsync(id, dto, ct));

    [Authorize(Roles = AppRoles.Admin)]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _brandService.DeleteAsync(id, ct);
        return NoContent();
    }
}