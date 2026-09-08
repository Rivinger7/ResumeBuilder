using FluentValidation;

namespace ResumeBuilder.Application.Features.Entries.Objectives.Deletes;

public sealed class DeleteObjectiveEntryCommandValidator : AbstractValidator<DeleteObjectiveEntryCommand>
{
    public DeleteObjectiveEntryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
