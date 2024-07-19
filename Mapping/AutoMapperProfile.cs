using AutoMapper;
using MusicApi.DTO.RequestDTO;
using MusicApi.DTO.ResponseDTO;
using MusicApi.Model;

namespace MusicApi.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Da entità a DTO
            CreateMap<Artist, ArtistDTO>();
            CreateMap<Album, AlbumDTO>();
            CreateMap<Song, SongDTO>();

            // Da DTO a entità
            CreateMap<CreateArtistDTO, Artist>();
            CreateMap<CreateAlbumDTO, Album>();
            CreateMap<CreateSongDTO, Song>();
        }
    }
}
