using FluentValidation;

namespace ResumeBuilder.Application.Features.Entries.Languages.Deletes;

public sealed class DeleteLanguageEntryCommandValidator : AbstractValidator<DeleteLanguageEntryCommand>
{
    public DeleteLanguageEntryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
