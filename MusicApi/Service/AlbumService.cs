using AutoMapper;
using MusicApi.Abstract;
using MusicApi.DataAccessLayer;
using MusicApi.DTO.RequestDTO;
using MusicApi.DTO.ResponseDTO;
using MusicApi.Model;
using MusicApi.Utilities.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;

public class AlbumService : IAlbumService
{
    private readonly IGenericRepository<Album> _albumRepository;
    private readonly IGenericRepository<Artist> _artistRepository;
    private readonly IMapper _mapper;
    private readonly ISubject _subject;
    private readonly ICommandInvoker _invoker;

    public AlbumService(IGenericRepository<Album> albumRepository, IGenericRepository<Artist> artistRepository, IMapper mapper, ISubject subject, ICommandInvoker invoker)
    {
        _albumRepository = albumRepository;
        _artistRepository = artistRepository;
        _mapper = mapper;
        _subject = subject;
        _invoker = invoker;
    }

    public async Task<IEnumerable<AlbumDTO>> GetAllAlbumsAsync()
    {
        var albums = await _albumRepository.GetAllIncludingAsync(album => album.Artist);
        return _mapper.Map<IEnumerable<AlbumDTO>>(albums);
    }

    public async Task<AlbumDTO> GetAlbumByIdAsync(int albumId)
    {
        var album = await _albumRepository.GetByIdAsync(albumId);
        if (album == null)
        {
            return null;
        }

        return _mapper.Map<AlbumDTO>(album);
    }

    public async Task<AlbumDTO> CreateAlbumAsync(CreateAlbumDTO albumDto)
    {
        var command = new CreateAlbumCommand(_albumRepository, _mapper, albumDto);
        _invoker.AddCommand(command);
        await _invoker.ExecuteCommandsAsync();

        var createdAlbum = command.CreatedAlbum;
        _subject.Notify($"Album created: {createdAlbum.Name}");
        return _mapper.Map<AlbumDTO>(createdAlbum);
    }

    public async Task UpdateAlbumAsync(int albumId, CreateAlbumDTO albumDto)
    {
        var command = new UpdateAlbumCommand(_albumRepository, _mapper, albumId, albumDto);
        _invoker.AddCommand(command);
        await _invoker.ExecuteCommandsAsync();

        var album = await _albumRepository.GetByIdAsync(albumId);
        _subject.Notify($"Album updated: {album.Name}");
    }

    public async Task DeleteAlbumAsync(int albumId)
    {
        var command = new DeleteAlbumCommand(_albumRepository, albumId);
        _invoker.AddCommand(command);
        await _invoker.ExecuteCommandsAsync();

        _subject.Notify($"Album deleted: {albumId}");
    }
}
