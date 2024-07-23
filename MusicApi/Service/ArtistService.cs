using AutoMapper;
using MusicApi.Abstract;
using MusicApi.DTO.RequestDTO;
using MusicApi.DTO.ResponseDTO;
using MusicApi.Model;
using MusicApi.Utilities.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ArtistService : IArtistService
{
    private readonly IGenericRepository<Artist> _artistRepository;
    private readonly IMapper _mapper;
    private readonly ISubject _subject;
    private readonly ICommandInvoker _invoker;

    public ArtistService(IGenericRepository<Artist> artistRepository, IMapper mapper, ISubject subject, ICommandInvoker invoker)
    {
        _artistRepository = artistRepository;
        _mapper = mapper;
        _subject = subject;
        _invoker = invoker;
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
        var command = new CreateArtistCommand(_artistRepository, _mapper, artistDto);
        _invoker.AddCommand(command);
        await _invoker.ExecuteCommandsAsync();

        var createdArtist = command.CreatedArtist;
        _subject.Notify($"Artist created: {createdArtist.FirstName} {createdArtist.LastName}");
        return _mapper.Map<ArtistDTO>(createdArtist);
    }

    public async Task UpdateArtistAsync(int artistId, CreateArtistDTO artistDto)
    {
        var command = new UpdateArtistCommand(_artistRepository, _mapper, artistId, artistDto);
        _invoker.AddCommand(command);
        await _invoker.ExecuteCommandsAsync();

        var artist = await _artistRepository.GetByIdAsync(artistId);
        _subject.Notify($"Artist updated: {artist.FirstName} {artist.LastName}");
    }

    public async Task DeleteArtistAsync(int artistId)
    {
        var command = new DeleteArtistCommand(_artistRepository, artistId);
        _invoker.AddCommand(command);
        await _invoker.ExecuteCommandsAsync();

        _subject.Notify($"Artist deleted: {artistId}");
    }

    // Pagination
    public async Task<PagedResult<ArtistDetailDTO>> PaginateArtistsAsync(int pageNumber, int pageSize)
    {
        var pagedResult = await _artistRepository.PaginateAsync(pageNumber, pageSize, artist => artist.Albums);
        var mappedItems = _mapper.Map<IEnumerable<ArtistDetailDTO>>(pagedResult.Items);

        return new PagedResult<ArtistDetailDTO>
        {
            Items = mappedItems,
            TotalItems = pagedResult.TotalItems,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

}
