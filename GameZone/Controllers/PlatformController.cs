using GameZone.DTOs.Platform;
using GameZone.DependencyInjection.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameZone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformController : ControllerBase
    {
        private readonly IPlatformService _platformService;

        public PlatformController(IPlatformService platformService)
        {
            _platformService = platformService;
        }

        [HttpGet("getall")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _platformService.GetAll());
        }

        [HttpGet("get-by{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var platform = await _platformService.GetById(id);

            if (platform == null)
                return NotFound();

            return Ok(platform);
        }

        [HttpPost("add-new-platform")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreatePlatformRequest request)
        {
            var platform = await _platformService.Create(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = platform.Id },
                platform
            );
        }

        [HttpPut("update{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(
            int id,
            UpdatePlatformRequest request)
        {
            await _platformService.Update(id, request);
            return NoContent();
        }

        [HttpDelete("delete-platform{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _platformService.Delete(id);
            return NoContent();
        }
    }
}
