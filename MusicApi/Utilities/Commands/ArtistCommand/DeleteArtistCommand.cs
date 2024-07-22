using MusicApi.Abstract;
using MusicApi.Model;
using System.Threading.Tasks;

namespace MusicApi.Utilities.Commands
{
    public class DeleteArtistCommand : ICommand
    {
        private readonly IGenericRepository<Artist> _artistRepository;
        private readonly int _artistId;

        public DeleteArtistCommand(IGenericRepository<Artist> artistRepository, int artistId)
        {
            _artistRepository = artistRepository;
            _artistId = artistId;
        }

        public async Task ExecuteAsync()
        {
            await _artistRepository.DeleteAsync(_artistId);
        }
    }
}
