using FluentValidation;

namespace ResumeBuilder.Application.Features.Entries.Skills.Updates;

public sealed class UpdateSkillEntryCommandValidator : AbstractValidator<UpdateSkillEntryCommand>
{
    public UpdateSkillEntryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
