using FluentValidation;
using ResumeBuilder.Domain.Enums;

namespace ResumeBuilder.Application.Features.Entries.Skills.Creates;

public sealed class CreateSkillEntryCommandValidator : AbstractValidator<CreateSkillEntryCommand>
{
    public CreateSkillEntryCommandValidator()
    {
        RuleFor(x => x.ResumeSectionId).NotEmpty();

        RuleFor(x => x).Custom((model, context) =>
        {
            switch (model.SkillLayout)
            {
                case LayoutType.Grid:
                    if (model.SkillGridColumn == null)
                    {
                        context.AddFailure(nameof(model.SkillGridColumn), "SkillGridColumn is required when layout is Grid.");
                    }
                    break;
                case LayoutType.Rows:
                    if (string.IsNullOrWhiteSpace(model.SkillRowSpacing))
                    {
                        context.AddFailure(nameof(model.SkillRowSpacing), "SkillRowSpacing is required when layout is Rows.");
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
