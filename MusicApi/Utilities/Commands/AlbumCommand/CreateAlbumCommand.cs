using AutoMapper;
using MusicApi.Abstract;
using MusicApi.DTO.RequestDTO;
using MusicApi.Model;
using System.Threading.Tasks;

namespace MusicApi.Utilities.Commands
{
    public class CreateAlbumCommand : ICommand
    {
        private readonly IGenericRepository<Album> _albumRepository;
        private readonly IMapper _mapper;
        private readonly CreateAlbumDTO _albumDto;
        public Album CreatedAlbum { get; private set; }

        public CreateAlbumCommand(IGenericRepository<Album> albumRepository, IMapper mapper, CreateAlbumDTO albumDto)
        {
            _albumRepository = albumRepository;
            _mapper = mapper;
            _albumDto = albumDto;
        }

        public async Task ExecuteAsync()
        {
            var album = _mapper.Map<Album>(_albumDto);
            await _albumRepository.AddAsync(album);
            CreatedAlbum = album;
        }
    }
}
