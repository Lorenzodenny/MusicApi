using AutoMapper;
using MusicApi.Abstract;
using MusicApi.DTO.RequestDTO;
using MusicApi.Model;
using System.Threading.Tasks;

namespace MusicApi.Utilities.Commands
{
    public class UpdateArtistCommand : ICommand
    {
        private readonly IGenericRepository<Artist> _artistRepository;
        private readonly IMapper _mapper;
        private readonly int _artistId;
        private readonly CreateArtistDTO _artistDto;

        public UpdateArtistCommand(IGenericRepository<Artist> artistRepository, IMapper mapper, int artistId, CreateArtistDTO artistDto)
        {
            _artistRepository = artistRepository;
            _mapper = mapper;
            _artistId = artistId;
            _artistDto = artistDto;
        }

        public async Task ExecuteAsync()
        {
            var artist = await _artistRepository.GetByIdAsync(_artistId);
            if (artist == null)
            {
                throw new ArgumentException("Artist not found.");
            }
            _mapper.Map(_artistDto, artist);
            await _artistRepository.UpdateAsync(artist);
        }
    }
}
