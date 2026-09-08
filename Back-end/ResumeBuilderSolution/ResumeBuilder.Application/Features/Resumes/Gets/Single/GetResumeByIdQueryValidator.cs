using FluentValidation;

namespace ResumeBuilder.Application.Features.Resumes.Gets.Single;

public sealed class GetResumeByIdQueryValidator : AbstractValidator<GetResumeByIdQuery>
{
    public GetResumeByIdQueryValidator()
    {
        RuleFor(x => x.ResumeId).NotEmpty();

        RuleFor(x => x.UserId).NotEmpty();
    }
}
