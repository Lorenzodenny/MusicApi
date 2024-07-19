namespace MusicApi.DTO.ResponseDTO
{
    public class AlbumDTO
    {
        public int AlbumId { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public string ArtistFullName { get; set; }
        public int SongsCount { get; set; }
    }
}
