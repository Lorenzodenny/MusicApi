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

            // Per la pagination di Song
            CreateMap<Song, SongDetailDTO>()
                .ForMember(dto => dto.AlbumName, conf => conf.MapFrom(song => song.Album.Name))
                .ForMember(dto => dto.ArtistName, conf => conf.MapFrom(song => $"{song.Album.Artist.FirstName} {song.Album.Artist.LastName}"));


            // Per la pagination di Album
            CreateMap<Album, AlbumDetailDTO>()
                .ForMember(dest => dest.ArtistFullName, opt => opt.MapFrom(src => src.Artist.FirstName + " " + src.Artist.LastName))
                .ForMember(dest => dest.SongsCount, opt => opt.MapFrom(src => src.Songs.Count))
                .ForMember(dest => dest.SongTitles, opt => opt.MapFrom(src => src.Songs.Select(s => s.Name).ToList()));

            // Per la pagination di Artist
            CreateMap<Artist, ArtistDetailDTO>()
                .ForMember(dest => dest.AlbumTitles, opt => opt.MapFrom(src => src.Albums.Select(a => a.Name).ToList()))
                .ForMember(dest => dest.AlbumsCount, opt => opt.MapFrom(src => src.Albums.Count));


            // Da DTO a entità
            CreateMap<CreateArtistDTO, Artist>();
            CreateMap<CreateAlbumDTO, Album>();
            CreateMap<CreateSongDTO, Song>();
        }
    }
}
