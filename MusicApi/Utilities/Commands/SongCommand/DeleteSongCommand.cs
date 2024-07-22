using MusicApi.Abstract;
using MusicApi.Model;
using System.Threading.Tasks;

namespace MusicApi.Utilities.Commands.SongCommand
{
    public class DeleteSongCommand : ICommand
    {
        private readonly IGenericRepository<Song> _songRepository;
        private readonly int _songId;

        public DeleteSongCommand(IGenericRepository<Song> songRepository, int songId)
        {
            _songRepository = songRepository;
            _songId = songId;
        }

        public async Task ExecuteAsync()
        {
            await _songRepository.DeleteAsync(_songId);
        }
    }
}
