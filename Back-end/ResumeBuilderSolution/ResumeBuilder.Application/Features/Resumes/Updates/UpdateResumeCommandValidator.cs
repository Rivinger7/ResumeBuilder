using FluentValidation;

namespace ResumeBuilder.Application.Features.Resumes.Updates;

public sealed class UpdateResumeCommandValidator : AbstractValidator<UpdateResumeCommand>
{
    public UpdateResumeCommandValidator()
    {
    }
}