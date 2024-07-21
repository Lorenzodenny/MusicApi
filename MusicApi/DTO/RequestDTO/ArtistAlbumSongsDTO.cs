namespace MusicApi.DTO.RequestDTO
{
    public class ArtistAlbumSongsDTO
    {
        public CreateArtistDTO ArtistDto { get; set; }
        public CreateAlbumDTO AlbumDto { get; set; }
        public List<CreateSongDTO> SongsDto { get; set; }
    }


}
