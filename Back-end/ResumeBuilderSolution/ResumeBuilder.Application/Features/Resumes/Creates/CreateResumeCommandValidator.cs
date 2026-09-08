using FluentValidation;

namespace ResumeBuilder.Application.Features.Resumes.Creates;

public sealed class CreateResumeCommandValidator : AbstractValidator<CreateResumeCommand>
{
    public CreateResumeCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();

        RuleFor(x => x.Title).NotEmpty().MaximumLength(255);

        RuleFor(x => x.Description).MaximumLength(2000);
    }
}