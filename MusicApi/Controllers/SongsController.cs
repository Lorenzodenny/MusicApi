using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MusicApi.Abstract;
using MusicApi.DTO.RequestDTO;
using MusicApi.Model;
using System;
using System.Threading.Tasks;

namespace MusicApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SongsController : ControllerBase
    {
        private readonly ISongOperation _songOperation; // Decorator
        private readonly ISongService _songService;

        public SongsController(ISongOperation songOperation, ISongService songService)
        {
            _songOperation = songOperation;
            _songService = songService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var songs = await _songService.GetAllSongsAsync();
                return Ok(songs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving songs.");
            }
        }

        // Qui uso il decorator
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var song = await _songOperation.GetSongByIdAsync(id); // decorator
                if (song == null) return NotFound();
                return Ok(song);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving the song.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSongDTO songDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdSong = await _songService.CreateSongAsync(songDto);
                return CreatedAtAction(nameof(GetById), new { id = createdSong.SongId }, createdSong);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while creating the song.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateSongDTO songDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _songService.UpdateSongAsync(id, songDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the song.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _songService.DeleteSongAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the song.");
            }
        }

        // Pagination
        [HttpGet("paginate")]
        public async Task<IActionResult> PaginateSongs([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _songService.PaginateSongsAsync(pageNumber, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while attempting to retrieve paginated songs.");
            }
        }



    }
}
