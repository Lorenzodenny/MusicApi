using MusicApi.Abstract;
using MusicApi.DTO.ResponseDTO;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MusicApi.DTO.RequestDTO;
using MusicApi.Model;

namespace MusicApi.Utilities.Proxies
{
    public class CachingSongServiceProxy : ISongService
    {
        private readonly ISongService _wrappedService;
        private readonly IMemoryCache _cache;

        public CachingSongServiceProxy(ISongService wrappedService, IMemoryCache cache)
        {
            _wrappedService = wrappedService;
            _cache = cache;
        }

        public async Task<IEnumerable<SongDTO>> GetAllSongsAsync()
        {
            if (_cache.TryGetValue("songs_all", out IEnumerable<SongDTO> cachedSongs))
            {
                return cachedSongs;
            }

            var songs = await _wrappedService.GetAllSongsAsync();
            _cache.Set("songs_all", songs, TimeSpan.FromMinutes(10)); // Cache for 10 minutes
            return songs;
        }

        public async Task<SongDTO> GetSongByIdAsync(int songId)
        {
            string cacheKey = $"song_{songId}";
            if (_cache.TryGetValue(cacheKey, out SongDTO cachedSong))
            {
                return cachedSong;
            }

            var song = await _wrappedService.GetSongByIdAsync(songId);
            _cache.Set(cacheKey, song, TimeSpan.FromMinutes(5)); // Cache for 5 minutes
            return song;
        }

        // Operazioni di scrittura non necessitano di caching
        public async Task<SongDTO> CreateSongAsync(CreateSongDTO songDto)
        {
            return await _wrappedService.CreateSongAsync(songDto);
        }

        public async Task UpdateSongAsync(int songId, CreateSongDTO songDto)
        {
            await _wrappedService.UpdateSongAsync(songId, songDto);
        }

        public async Task DeleteSongAsync(int songId)
        {
            await _wrappedService.DeleteSongAsync(songId);
        }

        // Pagination
        public async Task<PagedResult<SongDetailDTO>> PaginateSongsAsync(int pageNumber, int pageSize)
        {
            // Aggiungi qui la logica di caching se necessaria, altrimenti semplicemente chiama il metodo del servizio sottostante.
            return await _wrappedService.PaginateSongsAsync(pageNumber, pageSize);
        }

    }
}
