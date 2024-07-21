using MusicApi.Abstract;
using MusicApi.DTO.RequestDTO;
using MusicApi.DTO.ResponseDTO;

namespace MusicApi.Service.Facade
{
    public class MusicManagementFacade : IMusicManagementFacade
    {
        private readonly IArtistService _artistService;
        private readonly IAlbumService _albumService;
        private readonly ISongService _songService;

        public MusicManagementFacade(IArtistService artistService, IAlbumService albumService, ISongService songService)
        {
            _artistService = artistService;
            _albumService = albumService;
            _songService = songService;
        }

        public async Task AddArtistWithAlbumAndSongs(CreateArtistDTO artistDto, CreateAlbumDTO albumDto, IEnumerable<CreateSongDTO> songsDto)
        {
            // Aggiungi artista
            var artistResult = await _artistService.CreateArtistAsync(artistDto);

            // Imposta l'ID dell'artista nell'album
            albumDto.ArtistId = artistResult.ArtistId;

            // Aggiungi album
            var albumResult = await _albumService.CreateAlbumAsync(albumDto);

            // Aggiungi canzoni all'album
            foreach (var songDto in songsDto)
            {
                songDto.AlbumId = albumResult.AlbumId;
                await _songService.CreateSongAsync(songDto);
            }
        }

        // Creare un artista con vari album e varie song
        public async Task AddArtistWithMultipleAlbumsAndSongs(CreateArtistDTO artistDto, IEnumerable<CreateAlbumSongsDTO> albumsSongsDto)
        {
            // Aggiungi artista
            var artistResult = await _artistService.CreateArtistAsync(artistDto);

            // Itera su ogni album con le sue canzoni
            foreach (var albumSongs in albumsSongsDto)
            {
                // Imposta l'ID dell'artista nell'album
                albumSongs.AlbumDto.ArtistId = artistResult.ArtistId;

                // Aggiungi album
                var albumResult = await _albumService.CreateAlbumAsync(albumSongs.AlbumDto);

                // Aggiungi canzoni all'album
                foreach (var songDto in albumSongs.SongsDto)
                {
                    songDto.AlbumId = albumResult.AlbumId;
                    await _songService.CreateSongAsync(songDto);
                }
            }
        }

    }

}
