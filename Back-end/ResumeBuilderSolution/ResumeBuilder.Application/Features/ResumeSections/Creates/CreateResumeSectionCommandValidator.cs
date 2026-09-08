using FluentValidation;
using ResumeBuilder.Domain.Enums;

namespace ResumeBuilder.Application.Features.ResumeSections.Creates;

public sealed class CreateResumeSectionCommandValidator : AbstractValidator<CreateResumeSectionCommand>
{
    public CreateResumeSectionCommandValidator()
    {
        RuleFor(x => x.ResumeId).NotEmpty();

        RuleFor(x => x.Type).NotEmpty();

        RuleFor(x => x.Type)
            .NotEqual(ResumeSectionType.PersonalInformation)
            .WithMessage("Personal Information is created automatically, cannot create manually and only one exists");

        RuleFor(x => x.DisplayOrder)
            .NotEmpty()
            .Must(x => x >= 2)
            .WithMessage("Display Order must great than  or equal to 2");
    }
}