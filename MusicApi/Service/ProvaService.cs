using AutoMapper;
using MusicApi.Abstract;
using MusicApi.DTO.RequestDTO;
using MusicApi.Model;
using MusicApi.Utilities.Commands.SongCommand;
using MusicApi.Utilities.Commands;
using System.Threading.Tasks;

public class ProvaService
{
    private readonly ICommandInvoker _invoker;
    private readonly IGenericRepository<Artist> _artistRepository;
    private readonly IGenericRepository<Album> _albumRepository;
    private readonly IGenericRepository<Song> _songRepository;
    private readonly IMapper _mapper;

    public ProvaService(ICommandInvoker invoker, IGenericRepository<Artist> artistRepository, IGenericRepository<Album> albumRepository, IGenericRepository<Song> songRepository, IMapper mapper)
    {
        _invoker = invoker;
        _artistRepository = artistRepository;
        _albumRepository = albumRepository;
        _songRepository = songRepository;
        _mapper = mapper;
    }

    public async Task ExecuteProvaCommandsAsync(CreateArtistDTO artistDto, CreateAlbumDTO albumDto, CreateSongDTO songDto)
    {
        // Crea un comando per creare un artista
        var createArtistCommand = new CreateArtistCommand(_artistRepository, _mapper, artistDto);
        _invoker.AddCommand(createArtistCommand);

        // Crea un comando per creare un album
        var createAlbumCommand = new CreateAlbumCommand(_albumRepository, _mapper, albumDto);
        _invoker.AddCommand(createAlbumCommand);

        // Crea un comando per creare una canzone
        var createSongCommand = new CreateSongCommand(_songRepository, _mapper, songDto);
        _invoker.AddCommand(createSongCommand);

        // Esegui tutti i comandi
        await _invoker.ExecuteCommandsAsync();
    }
}
