using AutoMapper;
using MusicApi.Abstract;
using MusicApi.DTO.ResponseDTO;
using MusicApi.Model;

namespace MusicApi.Service
{
    public class BasicSongOperation : ISongOperation
    {
        private readonly IGenericRepository<Song> _songRepository;
        private readonly IMapper _mapper;

        public BasicSongOperation(IGenericRepository<Song> songRepository, IMapper mapper)
        {
            _songRepository = songRepository;
            _mapper = mapper;
        }

        public async Task<SongDTO> GetSongByIdAsync(int songId)
        {
            var song = await _songRepository.GetByIdAsync(songId);
            return song != null ? _mapper.Map<SongDTO>(song) : null;
        }
    }

}
