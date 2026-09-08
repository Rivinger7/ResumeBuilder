using FluentValidation;
using ResumeBuilder.Domain.Enums;

namespace ResumeBuilder.Application.Features.Entries.Languages.Creates;

public sealed class CreateLanguageEntryCommandValidator : AbstractValidator<CreateLanguageEntryCommand>
{
    public CreateLanguageEntryCommandValidator()
    {
        RuleFor(x => x.ResumeSectionId).NotEmpty();

        RuleFor(x => x).Custom((model, context) =>
        {
            switch (model.LanguageLayout)
            {
                case LayoutType.Grid:
                    if (model.LanguageGridColumn == null)
                    {
                        context.AddFailure(nameof(model.LanguageGridColumn), "LanguageGridColumn is required when layout is Grid.");
                    }

                    break;
                case LayoutType.Rows:
                    if (string.IsNullOrWhiteSpace(model.LanguageRowSpacing))
                    {
                        context.AddFailure(nameof(model.LanguageRowSpacing), "LanguageRowSpacing is required when layout is Rows.");
                    }

                    if (string.IsNullOrWhiteSpace(model.SubinfoStyle))
                    {
                        context.AddFailure(nameof(model.SubinfoStyle), "SubinfoStyle is required when layout is Rows.");
                    }

                    break;
            }
        });
    }
}