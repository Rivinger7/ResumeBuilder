using FluentValidation;

namespace ResumeBuilder.Application.Features.Resumes.Pdfs.Exports;

public sealed class ExportResumePdfQueryValidator : AbstractValidator<ExportResumePdfQuery>
{
    public ExportResumePdfQueryValidator()
    {
        RuleFor(x => x.ResumeId).NotEmpty();

        RuleFor(x => x.UserId).NotEmpty();
    }
}
