using AutoMapper;
using MusicApi.Abstract;
using MusicApi.DTO.RequestDTO;
using MusicApi.Model;
using System.Threading.Tasks;

namespace MusicApi.Utilities.Commands.SongCommand
{
    public class UpdateSongCommand : ICommand
    {
        private readonly IGenericRepository<Song> _songRepository;
        private readonly IMapper _mapper;
        private readonly int _songId;
        private readonly CreateSongDTO _songDto;

        public UpdateSongCommand(IGenericRepository<Song> songRepository, IMapper mapper, int songId, CreateSongDTO songDto)
        {
            _songRepository = songRepository;
            _mapper = mapper;
            _songId = songId;
            _songDto = songDto;
        }

        public async Task ExecuteAsync()
        {
            var song = await _songRepository.GetByIdAsync(_songId);
            if (song != null)
            {
                _mapper.Map(_songDto, song);
                await _songRepository.UpdateAsync(song);
            }
        }
    }
}
