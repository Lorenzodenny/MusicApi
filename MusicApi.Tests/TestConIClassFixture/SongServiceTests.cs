using AutoMapper;
using MusicApi.Abstract;
using MusicApi.DTO.RequestDTO;
using MusicApi.Mapping;
using MusicApi.Model;
using MusicApi.Repositories;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApi.Tests.TestConIClassFixture
{
    public class SongServiceTests : IClassFixture<DatabaseFixture>
    {
        private readonly SongService _service;
        public SongServiceTests(DatabaseFixture fixture)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });
            var mapper = new Mapper(config);

            _service = new SongService(new GenericRepository<Song>(fixture.Context), mapper);
        }
        // testo la get
        [Fact]
        public async Task GetAllSongs_ReturnsSongs()
        {
            var results = await _service.GetAllSongsAsync();
            Assert.NotEmpty(results);
        }

        // Testo la Get per Id
        [Fact]
        public async Task GetSongById_ReturnsCorrectSong()
        {
            var result = await _service.GetSongByIdAsync(1); // Assumi che l'ID 1 esista
            Assert.NotNull(result);
            Assert.Equal("Test Song 1", result.Name);
        }

        // Testo la Post
        [Fact]
        public async Task CreateSong_AddsSongCorrectly()
        {
            var newSong = new CreateSongDTO { Name = "New Test Song", Year = 2021, AlbumId = 1 };
            var createdSong = await _service.CreateSongAsync(newSong);
            Assert.NotNull(createdSong);
            Assert.Equal("New Test Song", createdSong.Name);
        }

        // Testo l'Update
        [Fact]
        public async Task UpdateSong_UpdatesSongCorrectly()
        {
            var songToUpdate = new CreateSongDTO { Name = "Updated Test Song", Year = 2020, AlbumId = 1 };
            await _service.UpdateSongAsync(1, songToUpdate); // Assumi che l'ID 1 esista
            var updatedSong = await _service.GetSongByIdAsync(1);
            Assert.NotNull(updatedSong);
            Assert.Equal("Updated Test Song", updatedSong.Name);
        }

        // Testo la Delete
        [Fact]
        public async Task DeleteSong_DeletesSongCorrectly()
        {
            await _service.DeleteSongAsync(1); // Assumi che l'ID 1 esista
            var deletedSong = await _service.GetSongByIdAsync(1);
            Assert.Null(deletedSong);
        }




    }
}
