using AutoMapper;
using MusicApi.Abstract;
using MusicApi.DTO.RequestDTO;
using MusicApi.DTO.ResponseDTO;
using MusicApi.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ArtistService : IArtistService
{
    private readonly IGenericRepository<Artist> _artistRepository;
    private readonly IMapper _mapper;

    public ArtistService(IGenericRepository<Artist> artistRepository, IMapper mapper)
    {
        _artistRepository = artistRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ArtistDTO>> GetAllArtistsAsync()
    {
        var artists = await _artistRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<ArtistDTO>>(artists);
    }

    public async Task<ArtistDTO> GetArtistByIdAsync(int artistId)
    {
        var artist = await _artistRepository.GetByIdAsync(artistId);
        if (artist == null)
        {
            return null;
        }

        return _mapper.Map<ArtistDTO>(artist);
    }

    public async Task<ArtistDTO> CreateArtistAsync(CreateArtistDTO artistDto)
    {
        var artist = _mapper.Map<Artist>(artistDto);
        await _artistRepository.AddAsync(artist);

        // Carica manualmente gli album
        artist = await _artistRepository.GetByIdAsync(artist.ArtistId);

        return _mapper.Map<ArtistDTO>(artist);
    }

    public async Task UpdateArtistAsync(int artistId, CreateArtistDTO artistDto)
    {
        var artist = await _artistRepository.GetByIdAsync(artistId);
        if (artist == null)
        {
            throw new ArgumentException("Artist not found.");
        }

        _mapper.Map(artistDto, artist);
        await _artistRepository.UpdateAsync(artist);
    }

    public async Task DeleteArtistAsync(int artistId)
    {
        await _artistRepository.DeleteAsync(artistId);
    }
}
