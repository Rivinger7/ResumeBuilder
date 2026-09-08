using FluentValidation;

namespace ResumeBuilder.Application.Features.Authentication.RefreshToken;

public sealed class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.OldRefreshToken)
            .NotEmpty();
    }
}