using FluentValidation;

namespace ResumeBuilder.Application.Features.Reorders.Entries.Updates;

public sealed class UpdateDisplayOrderEntryCommandValidator : AbstractValidator<UpdateDisplayOrderEntryCommand>
{
    public UpdateDisplayOrderEntryCommandValidator()
    {
        RuleFor(x => x.ResumeSectionId).NotEmpty();

        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.Type).IsInEnum();

        RuleFor(x => x.NewDisplayOrder).NotEmpty().GreaterThanOrEqualTo(0);
    }
}