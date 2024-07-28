using MusicApi.DTO.RequestDTO;
using MusicApi.DTO.ResponseDTO;
using MusicApi.Model;

namespace MusicApi.Abstract
{
    public interface ISongService
    {
        Task<IEnumerable<SongDTO>> GetAllSongsAsync(string name = null, int? year = null);
        Task<SongDTO> GetSongByIdAsync(int songId);
        Task<SongDTO> CreateSongAsync(CreateSongDTO song);
        Task UpdateSongAsync(int songId, CreateSongDTO song);
        Task DeleteSongAsync(int songId);

        // Pagination
        Task<PagedResult<SongDetailDTO>> PaginateSongsAsync(int pageNumber, int pageSize);
    }
}
