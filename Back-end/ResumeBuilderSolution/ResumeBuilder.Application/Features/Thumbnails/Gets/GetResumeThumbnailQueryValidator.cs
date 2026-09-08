using FluentValidation;

namespace ResumeBuilder.Application.Features.Thumbnails.Gets;

public sealed class GetResumeThumbnailQueryValidator : AbstractValidator<GetResumeThumbnailQuery>
{
    public GetResumeThumbnailQueryValidator()
    {
        RuleFor(x => x.ResumeId).NotEmpty();
    }
}
