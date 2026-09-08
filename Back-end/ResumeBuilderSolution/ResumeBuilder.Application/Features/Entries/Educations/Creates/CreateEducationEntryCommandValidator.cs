using FluentValidation;

namespace ResumeBuilder.Application.Features.Entries.Educations.Creates;

public sealed class CreateEducationEntryCommandValidator : AbstractValidator<CreateEducationEntryCommand>
{
    public CreateEducationEntryCommandValidator()
    {
        RuleFor(x => x.ResumeSectionId).NotEmpty();
    }
}