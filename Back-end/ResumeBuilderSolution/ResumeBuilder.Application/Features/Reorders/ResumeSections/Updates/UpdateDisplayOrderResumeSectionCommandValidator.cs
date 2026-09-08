using FluentValidation;

namespace ResumeBuilder.Application.Features.Reorders.ResumeSections.Updates;

public sealed class UpdateDisplayOrderResumeSectionCommandValidator : AbstractValidator<UpdateDisplayOrderResumeSectionCommand>
{
    public UpdateDisplayOrderResumeSectionCommandValidator()
    {
        RuleFor(x => x.ResumeId).NotEmpty();

        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.NewDisplayOrder).GreaterThanOrEqualTo(0);
    }
}