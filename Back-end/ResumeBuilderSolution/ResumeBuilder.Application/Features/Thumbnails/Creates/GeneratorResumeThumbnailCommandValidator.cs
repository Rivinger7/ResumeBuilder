using FluentValidation;

namespace ResumeBuilder.Application.Features.Thumbnails.Creates;

public sealed class GeneratorResumeThumbnailCommandValidator : AbstractValidator<GeneratorResumeThumbnailCommand>
{
    public GeneratorResumeThumbnailCommandValidator()
    {
        RuleFor(x => x.ResumeId).NotEmpty();
    }
}