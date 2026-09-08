using FluentValidation;

namespace ResumeBuilder.Application.Features.Entries.Experiences.Creates;

public sealed class CreateExperienceEntryCommandValidator : AbstractValidator<CreateExperienceEntryCommand>
{
    public CreateExperienceEntryCommandValidator()
    {
        RuleFor(x => x.ResumeSectionId).NotEmpty();
    }
}