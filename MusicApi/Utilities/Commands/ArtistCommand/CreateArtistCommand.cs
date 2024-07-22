using AutoMapper;
using MusicApi.Abstract;
using MusicApi.DTO.RequestDTO;
using MusicApi.Model;
using System.Threading.Tasks;

namespace MusicApi.Utilities.Commands
{
    public class CreateArtistCommand : ICommand
    {
        private readonly IGenericRepository<Artist> _artistRepository;
        private readonly IMapper _mapper;
        private readonly CreateArtistDTO _artistDto;
        public Artist CreatedArtist { get; private set; }

        public CreateArtistCommand(IGenericRepository<Artist> artistRepository, IMapper mapper, CreateArtistDTO artistDto)
        {
            _artistRepository = artistRepository;
            _mapper = mapper;
            _artistDto = artistDto;
        }

        public async Task ExecuteAsync()
        {
            var artist = _mapper.Map<Artist>(_artistDto);
            await _artistRepository.AddAsync(artist);
            CreatedArtist = artist;
        }
    }
}
