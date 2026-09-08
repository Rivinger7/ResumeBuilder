using FluentValidation;

namespace ResumeBuilder.Application.Features.Entries.PersonalInformations.Deletes;

public sealed class DeletePersonalInformationEntryCommandValidator : AbstractValidator<DeletePersonalInformationEntryCommand>
{
    public DeletePersonalInformationEntryCommandValidator()
    {
    }
}