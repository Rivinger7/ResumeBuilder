using FluentValidation;

namespace ResumeBuilder.Application.Features.Entries.PersonalInformations.Updates;

public sealed class UpdatePersonalInformationEntryCommandValidator : AbstractValidator<UpdatePersonalInformationEntryCommand>
{
    public UpdatePersonalInformationEntryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}