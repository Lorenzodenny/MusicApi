using Microsoft.AspNetCore.Mvc;
using MusicApi.Abstract;
using MusicApi.DTO.RequestDTO;
using System;
using System.Threading.Tasks;

namespace MusicApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArtistsController : ControllerBase
    {
        private readonly IArtistService _artistService;

        public ArtistsController(IArtistService artistService)
        {
            _artistService = artistService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var artists = await _artistService.GetAllArtistsAsync();
                return Ok(artists);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving artists.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var artist = await _artistService.GetArtistByIdAsync(id);
                if (artist == null) return NotFound();
                return Ok(artist);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving the artist.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateArtistDTO artistDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdArtist = await _artistService.CreateArtistAsync(artistDto);
                return CreatedAtAction(nameof(GetById), new { id = createdArtist.ArtistId }, createdArtist);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while creating the artist.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateArtistDTO artistDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _artistService.UpdateArtistAsync(id, artistDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the artist.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _artistService.DeleteArtistAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the artist.");
            }
        }

        // Pagination
        [HttpGet("paginate")]
        public async Task<IActionResult> PaginateArtists([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _artistService.PaginateArtistsAsync(pageNumber, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while attempting to retrieve paginated artists.");
            }
        }

    }
}
