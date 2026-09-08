using FluentValidation;

namespace ResumeBuilder.Application.Features.Entries.PersonalInformations.Updates;

public sealed class UpdatePersonalInformationEntryStyleCommandValidator : AbstractValidator<UpdatePersonalInformationEntryStyleCommand>
{
    public UpdatePersonalInformationEntryStyleCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.PersonalInformationTextAlignment)
            .IsInEnum();

        RuleFor(x => x.PersonalInformationLayout)
            .IsInEnum();
    }
}