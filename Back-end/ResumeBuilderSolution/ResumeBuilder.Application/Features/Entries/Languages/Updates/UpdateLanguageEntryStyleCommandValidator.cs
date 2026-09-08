using FluentValidation;
using ResumeBuilder.Domain.Enums;

namespace ResumeBuilder.Application.Features.Entries.Languages.Updates;

public sealed class UpdateLanguageEntryStyleCommandValidator : AbstractValidator<UpdateLanguageEntryStyleCommand>
{
    public UpdateLanguageEntryStyleCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

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
                    if (model.LanguageRowSpacing == null && model.IsStartRowsWithBullet == null && model.SubinfoStyle == null)
                    {
                        context.AddFailure(nameof(model.LanguageLayout), "At least one of the following is required when layout is Rows: LanguageRowSpacing, IsStartRowsWithBullet, SubinfoStyle.");
                    }
                    break;
            }
        });
    }
}
