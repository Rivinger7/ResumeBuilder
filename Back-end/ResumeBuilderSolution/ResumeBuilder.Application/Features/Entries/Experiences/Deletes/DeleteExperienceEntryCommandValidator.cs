using FluentValidation;

namespace ResumeBuilder.Application.Features.Entries.Experiences.Deletes;

public sealed class DeleteExperienceEntryCommandValidator : AbstractValidator<DeleteExperienceEntryCommand>
{
    public DeleteExperienceEntryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
