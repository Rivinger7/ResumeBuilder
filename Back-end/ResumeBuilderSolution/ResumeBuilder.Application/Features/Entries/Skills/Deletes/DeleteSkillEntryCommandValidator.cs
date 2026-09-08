using FluentValidation;

namespace ResumeBuilder.Application.Features.Entries.Skills.Deletes;

public sealed class DeleteSkillEntryCommandValidator : AbstractValidator<DeleteSkillEntryCommand>
{
    public DeleteSkillEntryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
