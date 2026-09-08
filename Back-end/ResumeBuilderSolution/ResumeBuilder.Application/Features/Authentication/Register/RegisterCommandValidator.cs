using FluentValidation;

namespace ResumeBuilder.Application.Features.Authentication.Register;

public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(3);

        RuleFor(x => x.FullName)
            .NotEmpty()
            .MinimumLength(3);
    }
}
