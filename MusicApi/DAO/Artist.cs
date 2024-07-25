namespace MusicApi.Model
{
    public class Artist
    {
        public int ArtistId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Album> Albums { get; set; }
    }
}
