using AutoMapper;
using MusicApi.DTO.RequestDTO;
using MusicApi.DTO.ResponseDTO;
using MusicApi.Model;

public class ArtistMappingProfile : Profile
{
    public ArtistMappingProfile()
    {
        CreateMap<Artist, ArtistDTO>();


        // Per la pagination di Artist
        CreateMap<Artist, ArtistDetailDTO>()
            .ForMember(dest => dest.AlbumTitles, opt => opt.MapFrom(src => src.Albums.Select(a => a.Name).ToList()))
            .ForMember(dest => dest.AlbumsCount, opt => opt.MapFrom(src => src.Albums.Count));


        // Da DTO a entità
        CreateMap<CreateArtistDTO, Artist>();  
    }
}
