using GameZone.Api.Configurations;
using GameZone.Api.DTOs;
using GameZone.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameZone.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryDto>>> GetAll(CancellationToken ct)
        => Ok(await _categoryService.GetAllAsync(ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoryDto>> GetById(int id, CancellationToken ct)
        => Ok(await _categoryService.GetByIdAsync(id, ct));

    [Authorize(Roles = AppRoles.Admin)]
    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Create(CreateCategoryDto dto, CancellationToken ct)
    {
        var created = await _categoryService.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [Authorize(Roles = AppRoles.Admin)]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<CategoryDto>> Update(int id, UpdateCategoryDto dto, CancellationToken ct)
        => Ok(await _categoryService.UpdateAsync(id, dto, ct));

    [Authorize(Roles = AppRoles.Admin)]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _categoryService.DeleteAsync(id, ct);
        return NoContent();
    }
}