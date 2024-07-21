namespace MusicApi.DTO.RequestDTO
{
    public class CreateAlbumDTO
    {
        public string Name { get; set; }
        public string Genre { get; set; }
        public int ArtistId { get; set; }
    }
}
