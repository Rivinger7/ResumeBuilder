using FluentValidation;

namespace ResumeBuilder.Application.Features.Entries.PersonalInformations.Creates;

public sealed class CreatePersonalInformationEntryCommandValidator : AbstractValidator<CreatePersonalInformationEntryCommand>
{
    public CreatePersonalInformationEntryCommandValidator()
    {
    }
}