using MusicApi.Abstract;
using MusicApi.Model;
using System.Threading.Tasks;

namespace MusicApi.Utilities.Commands
{
    public class DeleteAlbumCommand : ICommand
    {
        private readonly IGenericRepository<Album> _albumRepository;
        private readonly int _albumId;

        public DeleteAlbumCommand(IGenericRepository<Album> albumRepository, int albumId)
        {
            _albumRepository = albumRepository;
            _albumId = albumId;
        }

        public async Task ExecuteAsync()
        {
            await _albumRepository.DeleteAsync(_albumId);
        }
    }
}
