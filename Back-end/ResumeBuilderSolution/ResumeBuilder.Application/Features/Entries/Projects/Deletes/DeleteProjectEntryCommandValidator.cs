using FluentValidation;

namespace ResumeBuilder.Application.Features.Entries.Projects.Deletes;

public sealed class DeleteProjectEntryCommandValidator : AbstractValidator<DeleteProjectEntryCommand>
{
    public DeleteProjectEntryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
