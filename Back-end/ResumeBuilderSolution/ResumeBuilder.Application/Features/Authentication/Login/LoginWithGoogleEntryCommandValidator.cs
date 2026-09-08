using FluentValidation;

namespace ResumeBuilder.Application.Features.Authentication.Login;

public sealed class LoginWithGoogleEntryCommandValidator : AbstractValidator<LoginWithGoogleEntryCommand>
{
    public LoginWithGoogleEntryCommandValidator()
    {
        RuleFor(x => x.IdToken).NotEmpty();
    }
}