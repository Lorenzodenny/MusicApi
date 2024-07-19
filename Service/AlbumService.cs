using AutoMapper;
using MusicApi.Abstract;
using MusicApi.Model;
using MusicApi.DTO.RequestDTO;
using MusicApi.DTO.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class AlbumService : IAlbumService
{
    private readonly IGenericRepository<Album> _albumRepository;
    private readonly IGenericRepository<Artist> _artistRepository;
    private readonly IMapper _mapper;

    public AlbumService(IGenericRepository<Album> albumRepository, IGenericRepository<Artist> artistRepository, IMapper mapper)
    {
        _albumRepository = albumRepository;
        _artistRepository = artistRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AlbumDTO>> GetAllAlbumsAsync()
    {
        var albums = await _albumRepository.GetAllAsync();
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
        var artist = await _artistRepository.GetByIdAsync(albumDto.ArtistId);
        if (artist == null)
        {
            throw new ArgumentException("Artist not found.");
        }

        var album = _mapper.Map<Album>(albumDto);
        album.ArtistId = artist.ArtistId;
        await _albumRepository.AddAsync(album);

        return _mapper.Map<AlbumDTO>(album);
    }

    public async Task UpdateAlbumAsync(int albumId, CreateAlbumDTO albumDto)
    {
        var album = await _albumRepository.GetByIdAsync(albumId);
        if (album == null)
        {
            throw new ArgumentException("Album not found.");
        }

        _mapper.Map(albumDto, album);
        await _albumRepository.UpdateAsync(album);
    }

    public async Task DeleteAlbumAsync(int albumId)
    {
        await _albumRepository.DeleteAsync(albumId);
    }
}
