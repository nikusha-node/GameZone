using GameZone.Api.Configurations;
using GameZone.Api.DTOs;
using GameZone.Api.Helpers;
using GameZone.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameZone.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GamesController : ControllerBase
{
    private readonly IGameService _gameService;

    public GamesController(IGameService gameService)
    {
        _gameService = gameService;
    }

    // ---------- Public ----------

    [HttpGet]
    public async Task<ActionResult<PagedResult<GameListDto>>> GetAll(
        [FromQuery] GameQueryParams query, CancellationToken ct)
        => Ok(await _gameService.GetPagedAsync(query, ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GameResponseDto>> GetById(int id, CancellationToken ct)
        => Ok(await _gameService.GetByIdAsync(id, ct));

    [HttpGet("by-slug/{slug}")]
    public async Task<ActionResult<GameResponseDto>> GetBySlug(string slug, CancellationToken ct)
        => Ok(await _gameService.GetBySlugAsync(slug, ct));

    // ---------- Admin ----------

    [Authorize(Roles = AppRoles.Admin)]
    [HttpGet("admin")]
    public async Task<ActionResult<PagedResult<GameListDto>>> GetAllForAdmin(
        [FromQuery] GameQueryParams query, CancellationToken ct)
        => Ok(await _gameService.GetPagedForAdminAsync(query, ct));

    [Authorize(Roles = AppRoles.Admin)]
    [HttpPost]
    public async Task<ActionResult<GameResponseDto>> Create(CreateGameDto dto, CancellationToken ct)
    {
        var created = await _gameService.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [Authorize(Roles = AppRoles.Admin)]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<GameResponseDto>> Update(int id, UpdateGameDto dto, CancellationToken ct)
        => Ok(await _gameService.UpdateAsync(id, dto, ct));

    [Authorize(Roles = AppRoles.Admin)]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _gameService.DeleteAsync(id, ct);
        return NoContent();
    }
}