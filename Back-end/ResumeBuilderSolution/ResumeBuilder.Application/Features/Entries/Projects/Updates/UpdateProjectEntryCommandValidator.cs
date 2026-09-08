using FluentValidation;

namespace ResumeBuilder.Application.Features.Entries.Projects.Updates;

public sealed class UpdateProjectEntryCommandValidator : AbstractValidator<UpdateProjectEntryCommand>
{
    public UpdateProjectEntryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}