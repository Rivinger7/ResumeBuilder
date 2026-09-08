using FluentValidation;
using ResumeBuilder.Domain.Enums;

namespace ResumeBuilder.Application.Features.Entries.Certificates.Creates;

public sealed class CreateCertificateEntryCommandValidator : AbstractValidator<CreateCertificateEntryCommand>
{
    public CreateCertificateEntryCommandValidator()
    {
        RuleFor(x => x.ResumeSectionId).NotEmpty();

        RuleFor(x => x.CertificateUrl)
            .Must(x => Uri.IsWellFormedUriString(x, UriKind.Absolute))
            .WithMessage("CertificateUrl must be a valid URL.")
            .When(x => !string.IsNullOrEmpty(x.CertificateUrl));

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
                    if (string.IsNullOrWhiteSpace(model.CertificateRowSpacing))
                    {
                        context.AddFailure(nameof(model.CertificateRowSpacing), "CertificateRowSpacing is required when layout is Rows.");
                    }

                    if (string.IsNullOrWhiteSpace(model.SubinfoStyle))
                    {
                        context.AddFailure(nameof(model.SubinfoStyle), "SubinfoStyle is required when layout is Rows.");
                    }

                    break;

                default:
                    context.AddFailure(nameof(model.CertificateLayout), "Invalid CertificateLayout value.");
                    break;
            }
        });
    }
}