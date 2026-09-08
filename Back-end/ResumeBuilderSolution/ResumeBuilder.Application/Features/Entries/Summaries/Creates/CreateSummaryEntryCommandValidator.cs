using FluentValidation;

namespace ResumeBuilder.Application.Features.Entries.Summaries.Creates;

public sealed class CreateSummaryEntryCommandValidator : AbstractValidator<CreateSummaryEntryCommand>
{
    public CreateSummaryEntryCommandValidator()
    {
        RuleFor(x => x.ResumeSectionId).NotEmpty();
    }
}