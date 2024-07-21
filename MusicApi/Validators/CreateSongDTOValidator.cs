using FluentValidation;
using MusicApi.DTO.RequestDTO;

namespace MusicApi.Validators
{
    public class CreateSongDTOValidator : AbstractValidator<CreateSongDTO>
    {
        public CreateSongDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Song name is required")
                .Length(2, 100).WithMessage("Song name must be between 2 and 100 characters");

            RuleFor(x => x.Year)
                .GreaterThan(1900).WithMessage("Year must be greater than 1900");

            RuleFor(x => x.AlbumId)
                .GreaterThan(0).WithMessage("AlbumId is required and must be greater than 0");
        }
    }
}
