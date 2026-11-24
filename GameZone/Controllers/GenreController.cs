using GameZone.DTOs.Genre;
using GameZone.DependencyInjection.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameZone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpGet("getall")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _genreService.GetAll());
        }

        [HttpGet("get-by{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var genre = await _genreService.GetById(id);

            if (genre == null)
                return NotFound();

            return Ok(genre);
        }

        [HttpPost("create-genre")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateGenreRequest request)
        {
            var genre = await _genreService.Create(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = genre.Id },
                genre
            );
        }

        [HttpPut("update-genre{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(
            int id,
            UpdateGenreRequest request)
        {
            await _genreService.Update(id, request);
            return NoContent();
        }

        [HttpDelete("delete-genre{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _genreService.Delete(id);
            return NoContent();
        }
    }
}
