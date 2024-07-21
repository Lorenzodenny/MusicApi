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
            CreateMap<Album, AlbumDTO>()
                .ForMember(dest => dest.ArtistFullName,
                           opt => opt.MapFrom(src => $"{src.Artist.FirstName} {src.Artist.LastName}"))
                .ForMember(dest => dest.SongsCount,
                           opt => opt.MapFrom(src => src.Songs.Count));
            CreateMap<Song, SongDTO>()
                .ForMember(dto => dto.AlbumName, conf => conf.MapFrom(song => song.Album.Name));

            // Da DTO a entità
            CreateMap<CreateArtistDTO, Artist>();
            CreateMap<CreateAlbumDTO, Album>();
            CreateMap<CreateSongDTO, Song>();
        }
    }
}
