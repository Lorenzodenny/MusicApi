namespace MusicApi.Model
{
    public class Album
    {
        public int AlbumId { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public int ArtistId { get; set; }
        public Artist Artist { get; set; }
        public List<Song> Songs { get; set; }
    }
}
