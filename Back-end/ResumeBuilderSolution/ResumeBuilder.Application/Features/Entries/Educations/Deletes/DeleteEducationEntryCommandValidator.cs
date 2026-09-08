using FluentValidation;

namespace ResumeBuilder.Application.Features.Entries.Educations.Deletes;

public sealed class DeleteEducationEntryCommandValidator : AbstractValidator<DeleteEducationEntryCommand>
{
    public DeleteEducationEntryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
