using FluentValidation;

namespace ResumeBuilder.Application.Features.Entries.Projects.Creates;

public sealed class CreateProjectEntryCommandValidator : AbstractValidator<CreateProjectEntryCommand>
{
    public CreateProjectEntryCommandValidator()
    {
        RuleFor(x => x.ResumeSectionId).NotEmpty();

        RuleFor(x => x.ProjectUrl)
            .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
            .WithMessage("ProjectUrl must be a valid absolute URL.")
            .When(x => !string.IsNullOrWhiteSpace(x.ProjectUrl));

        RuleFor(x => x.RepositoryUrl)
            .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
            .WithMessage("RepositoryUrl must be a valid absolute URL.")
            .When(x => !string.IsNullOrWhiteSpace(x.RepositoryUrl));
    }
}