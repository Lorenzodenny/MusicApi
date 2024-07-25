using AutoMapper;
using MusicApi.DTO.RequestDTO;
using MusicApi.DTO.ResponseDTO;
using MusicApi.Model;

public class AlbumMappingProfile : Profile
{
    public AlbumMappingProfile()
    {
        // Da entità a DTO
        CreateMap<Album, AlbumDTO>()
            .ForMember(dest => dest.ArtistFullName, opt => opt.MapFrom(src => $"{src.Artist.FirstName} {src.Artist.LastName}"))
            .ForMember(dest => dest.SongsCount, opt => opt.MapFrom(src => src.Songs.Count));


        // Per la pagination di Album
        CreateMap<Album, AlbumDetailDTO>()
            .ForMember(dest => dest.ArtistFullName, opt => opt.MapFrom(src => src.Artist.FirstName + " " + src.Artist.LastName))
            .ForMember(dest => dest.SongsCount, opt => opt.MapFrom(src => src.Songs.Count))
            .ForMember(dest => dest.SongTitles, opt => opt.MapFrom(src => src.Songs.Select(s => s.Name).ToList()));

        // Da DTO a entità
        CreateMap<CreateAlbumDTO, Album>();
    }
}
