using FluentValidation;

namespace ResumeBuilder.Application.Features.Entries.Languages.Updates;

public sealed class UpdateLanguageEntryCommandValidator : AbstractValidator<UpdateLanguageEntryCommand>
{
    public UpdateLanguageEntryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}