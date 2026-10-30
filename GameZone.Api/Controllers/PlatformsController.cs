using GameZone.Api.Configurations;
using GameZone.Api.DTOs;
using GameZone.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameZone.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformService _platformService;

    public PlatformsController(IPlatformService platformService)
    {
        _platformService = platformService;
    }

    [HttpGet]
    public async Task<ActionResult<List<PlatformDto>>> GetAll(CancellationToken ct)
        => Ok(await _platformService.GetAllAsync(ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PlatformDto>> GetById(int id, CancellationToken ct)
        => Ok(await _platformService.GetByIdAsync(id, ct));

    [Authorize(Roles = AppRoles.Admin)]
    [HttpPost]
    public async Task<ActionResult<PlatformDto>> Create(CreatePlatformDto dto, CancellationToken ct)
    {
        var created = await _platformService.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [Authorize(Roles = AppRoles.Admin)]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<PlatformDto>> Update(int id, UpdatePlatformDto dto, CancellationToken ct)
        => Ok(await _platformService.UpdateAsync(id, dto, ct));

    [Authorize(Roles = AppRoles.Admin)]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _platformService.DeleteAsync(id, ct);
        return NoContent();
    }
}