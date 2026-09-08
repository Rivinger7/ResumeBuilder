using FluentValidation;

namespace ResumeBuilder.Application.Features.Entries.Summaries.Deletes;

public sealed class DeleteSummaryEntryCommandValidator : AbstractValidator<DeleteSummaryEntryCommand>
{
    public DeleteSummaryEntryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
