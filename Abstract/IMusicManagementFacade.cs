using Microsoft.AspNetCore.Mvc;
using MusicApi.DTO.RequestDTO;

namespace MusicApi.Abstract
{
    public interface IMusicManagementFacade
    {
        Task AddArtistWithAlbumAndSongs(CreateArtistDTO artistDto, CreateAlbumDTO albumDto, IEnumerable<CreateSongDTO> songsDto);

        Task AddArtistWithMultipleAlbumsAndSongs(CreateArtistDTO artistDto, IEnumerable<CreateAlbumSongsDTO> albumsSongsDto);
    }
}
