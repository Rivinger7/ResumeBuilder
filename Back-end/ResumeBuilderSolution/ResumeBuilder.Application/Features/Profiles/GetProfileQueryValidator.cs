using FluentValidation;

namespace ResumeBuilder.Application.Features.Profiles;

public sealed class GetProfileQueryValidator : AbstractValidator<GetProfileQuery>
{
    public GetProfileQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}