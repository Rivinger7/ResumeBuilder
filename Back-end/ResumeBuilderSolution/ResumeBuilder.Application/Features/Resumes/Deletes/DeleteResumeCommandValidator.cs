using FluentValidation;

namespace ResumeBuilder.Application.Features.Resumes.Deletes;

public sealed class DeleteResumeCommandValidator : AbstractValidator<DeleteResumeCommand>
{
    public DeleteResumeCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}