using Microsoft.AspNetCore.Mvc;
using MusicApi.Abstract;
using MusicApi.DTO.RequestDTO;
using MusicApi.Service.Facade;


namespace MusicApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MusicManagementController : ControllerBase
    {
        private readonly IMusicManagementFacade _musicManagementFacade;

        public MusicManagementController(IMusicManagementFacade musicManagementFacade)
        {
            _musicManagementFacade = musicManagementFacade;
        }

        [HttpPost("AddArtistWithAlbumsAndSongs")]
        public async Task<IActionResult> AddArtistWithAlbumsAndSongs([FromBody] ArtistAlbumSongsDTO dto)
        {
            await _musicManagementFacade.AddArtistWithAlbumAndSongs(dto.ArtistDto, dto.AlbumDto, dto.SongsDto);
            return Ok();
        }

        [HttpPost("AddArtistWithMultipleAlbumsAndSongs")]
        public async Task<IActionResult> AddArtistWithMultipleAlbumsAndSongs([FromBody] ArtistMultipleAlbumsSongsDTO dto)
        {
            await _musicManagementFacade.AddArtistWithMultipleAlbumsAndSongs(dto.ArtistDto, dto.AlbumsSongsDto);
            return Ok();
        }

    }
}
