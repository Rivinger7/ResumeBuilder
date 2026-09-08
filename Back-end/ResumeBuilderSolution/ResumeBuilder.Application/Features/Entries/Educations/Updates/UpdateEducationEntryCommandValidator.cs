using FluentValidation;

namespace ResumeBuilder.Application.Features.Entries.Educations.Updates;

public sealed class UpdateEducationEntryCommandValidator : AbstractValidator<UpdateEducationEntryCommand>
{
    public UpdateEducationEntryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}