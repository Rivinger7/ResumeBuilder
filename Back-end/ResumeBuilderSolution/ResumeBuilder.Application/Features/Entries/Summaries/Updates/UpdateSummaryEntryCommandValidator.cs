using FluentValidation;

namespace ResumeBuilder.Application.Features.Entries.Summaries.Updates;

public sealed class UpdateSummaryEntryCommandValidator : AbstractValidator<UpdateSummaryEntryCommand>
{
    public UpdateSummaryEntryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}