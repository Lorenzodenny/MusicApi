using MusicApi.DTO.RequestDTO;
using MusicApi.DTO.ResponseDTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicApi.Abstract
{
    public interface IArtistService
    {
        Task<IEnumerable<ArtistDTO>> GetAllArtistsAsync();
        Task<ArtistDTO> GetArtistByIdAsync(int artistId);
        Task<ArtistDTO> CreateArtistAsync(CreateArtistDTO artistDto);
        Task UpdateArtistAsync(int artistId, CreateArtistDTO artistDto);
        Task DeleteArtistAsync(int artistId);
    }
}
