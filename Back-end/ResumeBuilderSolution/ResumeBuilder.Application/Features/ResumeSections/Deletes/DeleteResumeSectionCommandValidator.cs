using FluentValidation;

namespace ResumeBuilder.Application.Features.ResumeSections.Deletes;

public sealed class DeleteResumeSectionCommandValidator : AbstractValidator<DeleteResumeSectionCommand>
{
    public DeleteResumeSectionCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
