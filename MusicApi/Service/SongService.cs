using AutoMapper;
using MusicApi.Abstract;
using MusicApi.DataAccessLayer;
using MusicApi.DTO.RequestDTO;
using MusicApi.DTO.ResponseDTO;
using MusicApi.Model;
using MusicApi.Utilities.Commands;
using MusicApi.Utilities.Commands.SongCommand;
using System.Collections.Generic;
using System.Threading.Tasks;

public class SongService : ISongService
{
    private readonly IGenericRepository<Song> _songRepository;
    private readonly IMapper _mapper;
    private readonly ISubject _subject;
    private readonly ICommandInvoker _invoker;

    public SongService(IGenericRepository<Song> songRepository, IMapper mapper, ISubject subject, ICommandInvoker invoker)
    {
        _songRepository = songRepository;
        _mapper = mapper;
        _subject = subject;
        _invoker = invoker;
    }

    public async Task<IEnumerable<SongDTO>> GetAllSongsAsync()
    {
        var songs = await _songRepository.GetAllIncludingAsync(song => song.Album);
        return _mapper.Map<IEnumerable<SongDTO>>(songs);
    }

    public async Task<SongDTO> GetSongByIdAsync(int songId)
    {
        var song = await _songRepository.GetByIdAsync(songId);
        return song != null ? _mapper.Map<SongDTO>(song) : null;
    }

    public async Task<SongDTO> CreateSongAsync(CreateSongDTO songDto)
    {
        var command = new CreateSongCommand(_songRepository, _mapper, songDto);
        _invoker.AddCommand(command);
        await _invoker.ExecuteCommandsAsync();

        var createdSong = command.CreatedSong;
        _subject.Notify($"Song created: {createdSong.Name}");
        return _mapper.Map<SongDTO>(createdSong);
    }


    public async Task UpdateSongAsync(int songId, CreateSongDTO songDto)
    {
        var command = new UpdateSongCommand(_songRepository, _mapper, songId, songDto);
        _invoker.AddCommand(command);
        await _invoker.ExecuteCommandsAsync();

        var song = await _songRepository.GetByIdAsync(songId);
        _subject.Notify($"Song updated: {song.Name}");
    }

    public async Task DeleteSongAsync(int songId)
    {
        var command = new DeleteSongCommand(_songRepository, songId);
        _invoker.AddCommand(command);
        await _invoker.ExecuteCommandsAsync();
        _subject.Notify($"Song deleted: {songId}");
    }

}
