using Microsoft.AspNetCore.Mvc;
using MusicApi.Abstract;
using MusicApi.DTO.RequestDTO;
using System;
using System.Threading.Tasks;

namespace MusicApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlbumsController : ControllerBase
    {
        private readonly IAlbumService _albumService;

        public AlbumsController(IAlbumService albumService)
        {
            _albumService = albumService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var albums = await _albumService.GetAllAlbumsAsync();
                return Ok(albums);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving albums.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var album = await _albumService.GetAlbumByIdAsync(id);
                if (album == null) return NotFound();
                return Ok(album);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving the album.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAlbumDTO albumDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdAlbum = await _albumService.CreateAlbumAsync(albumDto);
                return CreatedAtAction(nameof(GetById), new { id = createdAlbum.AlbumId }, createdAlbum);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while creating the album.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateAlbumDTO albumDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _albumService.UpdateAlbumAsync(id, albumDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the album.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _albumService.DeleteAlbumAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the album.");
            }
        }

        // Pagination
        [HttpGet("paginate")]
        public async Task<IActionResult> PaginateAlbumDetails(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var result = await _albumService.PaginateAlbumDetailsAsync(pageNumber, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while attempting to retrieve paginated album details.");
            }
        }

    }
}
