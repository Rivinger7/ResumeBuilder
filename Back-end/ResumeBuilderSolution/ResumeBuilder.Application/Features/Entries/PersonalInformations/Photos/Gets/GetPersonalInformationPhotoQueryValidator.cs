using FluentValidation;

namespace ResumeBuilder.Application.Features.Entries.PersonalInformations.Photos.Gets;

public sealed class GetPersonalInformationPhotoQueryValidator : AbstractValidator<GetPersonalInformationPhotoQuery>
{
    public GetPersonalInformationPhotoQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
