using Microsoft.AspNetCore.Mvc;
using MusicApi.DTO.RequestDTO;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ProvaController : ControllerBase
{
    private readonly ProvaService _provaService;

    public ProvaController(ProvaService provaService)
    {
        _provaService = provaService;
    }

    [HttpPost]
    [Route("execute")]
    public async Task<IActionResult> ExecuteProvaCommands([FromBody] ProvaCommandRequest request)
    {
        await _provaService.ExecuteProvaCommandsAsync(request.CreateArtistDto, request.CreateAlbumDto, request.CreateSongDto);
        return Ok("Commands executed successfully.");
    }
}


