using MusicApi.DTO.ResponseDTO;

namespace MusicApi.Abstract
{
    public interface ISongOperation
    {
        Task<SongDTO> GetSongByIdAsync(int songId);
    }

}
