namespace MusicApi.DTO.ResponseDTO
{
    public class ArtistDetailDTO
    {
        public int ArtistId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int AlbumsCount { get; set; }
        public List<string> AlbumTitles { get; set; } // Lista dei titoli degli album
    }
}
