using FluentValidation;

namespace ResumeBuilder.Application.Features.Resumes.Gets.Paged;

public sealed class GetPagedResumeQueryValidator : AbstractValidator<GetPagedResumeQuery>
{
    public GetPagedResumeQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();

        RuleFor(x => x.PageSize).NotEmpty().GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageNumber).NotEmpty().GreaterThanOrEqualTo(1);
    }
}