using AutoMapper;
using MusicApi.Abstract;
using MusicApi.DTO.RequestDTO;
using MusicApi.Model;
using System.Threading.Tasks;

namespace MusicApi.Utilities.Commands
{
    public class UpdateAlbumCommand : ICommand
    {
        private readonly IGenericRepository<Album> _albumRepository;
        private readonly IMapper _mapper;
        private readonly int _albumId;
        private readonly CreateAlbumDTO _albumDto;

        public UpdateAlbumCommand(IGenericRepository<Album> albumRepository, IMapper mapper, int albumId, CreateAlbumDTO albumDto)
        {
            _albumRepository = albumRepository;
            _mapper = mapper;
            _albumId = albumId;
            _albumDto = albumDto;
        }

        public async Task ExecuteAsync()
        {
            var album = await _albumRepository.GetByIdAsync(_albumId);
            if (album == null)
            {
                throw new ArgumentException("Album not found.");
            }
            _mapper.Map(_albumDto, album);
            await _albumRepository.UpdateAsync(album);
        }
    }
}
