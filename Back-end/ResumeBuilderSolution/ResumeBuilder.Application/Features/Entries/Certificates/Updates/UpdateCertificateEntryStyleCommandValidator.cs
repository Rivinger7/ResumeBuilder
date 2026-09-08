using FluentValidation;
using ResumeBuilder.Domain.Enums;

namespace ResumeBuilder.Application.Features.Entries.Certificates.Updates;

public sealed class UpdateCertificateEntryStyleCommandValidator : AbstractValidator<UpdateCertificateEntryStyleCommand>
{
    public UpdateCertificateEntryStyleCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x).Custom((model, context) =>
        {
            switch (model.CertificateLayout)
            {
                case LayoutType.Grid:
                    if (model.CertificateGridColumn == null)
                    {
                        context.AddFailure(nameof(model.CertificateGridColumn), "CertificateGridColumn is required when layout is Grid.");
                    }

                    break;

                case LayoutType.Rows:
                    if (model.CertificateRowSpacing == null && model.IsStartRowsWithBullet == null && model.SubinfoStyle == null)
                    {
                        context.AddFailure(nameof(model.CertificateLayout), "At least one of the following is required when layout is Rows: CertificateRowSpacing, IsStartRowsWithBullet, SubinfoStyle.");
                    }

                    break;
            }
        });
    }
}