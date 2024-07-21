namespace MusicApi.DTO.RequestDTO
{
    public class ArtistMultipleAlbumsSongsDTO
    {
        public CreateArtistDTO ArtistDto { get; set; }
        public IEnumerable<CreateAlbumSongsDTO> AlbumsSongsDto { get; set; }
    }

}
