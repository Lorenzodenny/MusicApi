using AutoMapper;
using MusicApi.Abstract;
using MusicApi.DTO.RequestDTO;
using MusicApi.DTO.ResponseDTO;
using MusicApi.Model;
using System.Threading.Tasks;

namespace MusicApi.Utilities.Commands.SongCommand
{
    public class CreateSongCommand : ICommand
    {
        private readonly IGenericRepository<Song> _songRepository;
        private readonly IMapper _mapper;
        private readonly CreateSongDTO _songDto;
        public Song CreatedSong { get; private set; }

        public CreateSongCommand(IGenericRepository<Song> songRepository, IMapper mapper, CreateSongDTO songDto)
        {
            _songRepository = songRepository;
            _mapper = mapper;
            _songDto = songDto;
        }

        public async Task ExecuteAsync()
        {
            var song = _mapper.Map<Song>(_songDto);
            await _songRepository.AddAsync(song);
            CreatedSong = song;
        }
    }
}
