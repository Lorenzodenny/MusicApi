using AutoMapper;
using MusicApi.DTO.RequestDTO;
using MusicApi.DTO.ResponseDTO;
using MusicApi.Model;

public class SongMappingProfile : Profile
{
    public SongMappingProfile()
    {
        // Da entità a DTO
        CreateMap<Song, SongDTO>()
            .ForMember(dto => dto.AlbumName, conf => conf.MapFrom(song => song.Album.Name));


        // Per la pagination di Song
        CreateMap<Song, SongDetailDTO>()
            .ForMember(dto => dto.AlbumName, conf => conf.MapFrom(song => song.Album.Name))
            .ForMember(dto => dto.ArtistName, conf => conf.MapFrom(song => $"{song.Album.Artist.FirstName} {song.Album.Artist.LastName}"));

        // Da DTO a entità
        CreateMap<CreateSongDTO, Song>();
    }
}
