namespace MusicApi.DTO.RequestDTO
{
    public class CreateAlbumSongsDTO
    {
        public CreateAlbumDTO AlbumDto { get; set; }
        public IEnumerable<CreateSongDTO> SongsDto { get; set; }
    }

}
