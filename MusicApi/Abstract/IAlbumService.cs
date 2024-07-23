using MusicApi.DTO.RequestDTO;
using MusicApi.DTO.ResponseDTO;
using MusicApi.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicApi.Abstract
{
    public interface IAlbumService
    {
        Task<IEnumerable<AlbumDTO>> GetAllAlbumsAsync();
        Task<AlbumDTO> GetAlbumByIdAsync(int albumId);
        Task<AlbumDTO> CreateAlbumAsync(CreateAlbumDTO albumDto);
        Task UpdateAlbumAsync(int albumId, CreateAlbumDTO albumDto);
        Task DeleteAlbumAsync(int albumId);

        // Pagination
        Task<PagedResult<AlbumDetailDTO>> PaginateAlbumDetailsAsync(int pageNumber, int pageSize);
    }
}
