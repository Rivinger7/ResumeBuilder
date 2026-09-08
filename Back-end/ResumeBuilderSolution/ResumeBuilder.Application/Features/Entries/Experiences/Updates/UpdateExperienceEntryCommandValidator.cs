using FluentValidation;

namespace ResumeBuilder.Application.Features.Entries.Experiences.Updates;

public sealed class UpdateExperienceEntryCommandValidator : AbstractValidator<UpdateExperienceEntryCommand>
{
    public UpdateExperienceEntryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}