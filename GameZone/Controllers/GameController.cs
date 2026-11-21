using GameZone.DependencyInjection.Interfaces;
using GameZone.DTOs.Game;
using GameZone.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameZone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(
             [FromQuery] string? search,
             [FromQuery] string? platform,
             [FromQuery] string? genre,
             [FromQuery] decimal? minPrice,
             [FromQuery] decimal? maxPrice,
             [FromQuery] bool? onSale,
             [FromQuery] AgeRating? ageRating,
             [FromQuery] string? sortBy,
             [FromQuery] int page = 1,
             [FromQuery] int pageSize = 8)
            {
            var games = await _gameService.GetAll(
                search,
                platform,
                genre,
                minPrice,
                maxPrice,
                onSale,
                ageRating,
                sortBy,
                page,
                pageSize
            );

            return Ok(games);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var game = await _gameService.GetById(id);

            if (game == null)
                return NotFound();

            return Ok(game);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateGameRequest request)
        {
            var game = await _gameService.Create(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = game.Id },
                game
            );
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(
            int id,
            UpdateGameRequest request)
        {
            await _gameService.Update(id, request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _gameService.Delete(id);
            return NoContent();
        }
    }
}
