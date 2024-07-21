using FluentValidation;
using MusicApi.DTO.RequestDTO;

namespace MusicApi.Validators
{
    public class CreateArtistDTOValidator : AbstractValidator<CreateArtistDTO>
    {
        public CreateArtistDTOValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .Length(2, 50).WithMessage("First name must be between 2 and 50 characters");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .Length(2, 50).WithMessage("Last name must be between 2 and 50 characters");
        }
    }

}
