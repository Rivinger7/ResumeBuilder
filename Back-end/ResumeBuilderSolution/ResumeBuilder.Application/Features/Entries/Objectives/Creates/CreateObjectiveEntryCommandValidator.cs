using FluentValidation;

namespace ResumeBuilder.Application.Features.Entries.Objectives.Creates;

public sealed class CreateObjectiveEntryCommandValidator : AbstractValidator<CreateObjectiveEntryCommand>
{
    public CreateObjectiveEntryCommandValidator()
    {
        RuleFor(x => x.ResumeSectionId).NotEmpty();
    }
}