using MusicApi.DTO.RequestDTO;
using MusicApi.DTO.ResponseDTO;

namespace MusicApi.Abstract
{
    public interface ISongService
    {
        Task<IEnumerable<SongDTO>> GetAllSongsAsync();
        Task<SongDTO> GetSongByIdAsync(int songId);
        Task<SongDTO> CreateSongAsync(CreateSongDTO song);
        Task UpdateSongAsync(int songId, CreateSongDTO song);
        Task DeleteSongAsync(int songId);
    }
}
