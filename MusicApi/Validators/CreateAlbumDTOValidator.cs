using FluentValidation;
using MusicApi.DTO.RequestDTO;

namespace MusicApi.Validators
{
    public class CreateAlbumDTOValidator : AbstractValidator<CreateAlbumDTO>
    {
        public CreateAlbumDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Album name is required")
                .Length(2, 50).WithMessage("Album name must be between 2 and 50 characters");

            RuleFor(x => x.Genre)
                .NotEmpty().WithMessage("Genre is required");

            RuleFor(x => x.ArtistId)
                .GreaterThan(0).WithMessage("ArtistId is required and must be greater than 0");
        }
    }
}
