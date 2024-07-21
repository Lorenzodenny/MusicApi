using Microsoft.AspNetCore.Mvc;
using MusicApi.Abstract;
using MusicApi.DTO.RequestDTO;
using MusicApi.Service.HTTP_Client;

namespace MusicApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FakeDataController : ControllerBase
    {
        private readonly FakeApiService _fakeApiService;
        private readonly ISongService _songService;

        public FakeDataController(FakeApiService fakeApiService, ISongService songService)
        {
            _fakeApiService = fakeApiService;
            _songService = songService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var data = await _fakeApiService.GetFakeDataAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error fetching data from fake API");
            }
        }

        [HttpPost("createSongFromTodo")]
        public async Task<IActionResult> CreateSongFromTodo()
        {
            try
            {
                var todoItem = await _fakeApiService.GetFakeDataAsync();
                var newSong = new CreateSongDTO
                {
                    Name = todoItem.Title,
                    Year = DateTime.Now.Year, // Supponiamo di mettere l'anno corrente
                    AlbumId = 1 // Assumi un AlbumId valido per semplicità
                };

                var createdSong = await _songService.CreateSongAsync(newSong);
                return CreatedAtAction("GetById", "Songs", new { id = createdSong.SongId }, createdSong);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error creating song from API data: " + ex.Message);
            }
        }

    }

}
