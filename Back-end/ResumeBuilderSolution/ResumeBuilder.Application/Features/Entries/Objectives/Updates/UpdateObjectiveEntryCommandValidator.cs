using FluentValidation;

namespace ResumeBuilder.Application.Features.Entries.Objectives.Updates;

public sealed class UpdateObjectiveEntryCommandValidator : AbstractValidator<UpdateObjectiveEntryCommand>
{
    public UpdateObjectiveEntryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}