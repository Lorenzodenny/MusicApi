namespace MusicApi.DTO.ResponseDTO
{
    public class AlbumDetailDTO
    {
        public int AlbumId { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public string ArtistFullName { get; set; }
        public int SongsCount { get; set; }
        public List<string> SongTitles { get; set; }  
    }
}
