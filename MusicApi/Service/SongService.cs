using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MusicApi.Abstract;
using MusicApi.DataAccessLayer;
using MusicApi.DTO.RequestDTO;
using MusicApi.DTO.ResponseDTO;
using MusicApi.Model;

public class SongService : ISongService
{

    private readonly IGenericRepository<Song> _songRepository;
    private readonly IMapper _mapper;
    public SongService(IGenericRepository<Song> songRepository, IMapper mapper)
    {
        _songRepository = songRepository;
        _mapper = mapper;
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
        var song = _mapper.Map<Song>(songDto);
        await _songRepository.AddAsync(song);
        return _mapper.Map<SongDTO>(song);
    }


    public async Task UpdateSongAsync(int songId, CreateSongDTO songDto)
    {
        var song = await _songRepository.GetByIdAsync(songId);
        if (song == null) return;
        _mapper.Map(songDto, song);
        await _songRepository.UpdateAsync(song);
    }

    public async Task DeleteSongAsync(int songId)
    {
        await _songRepository.DeleteAsync(songId);
    }
}
