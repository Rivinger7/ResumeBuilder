using FluentValidation;

namespace ResumeBuilder.Application.Features.Entries.Certificates.Updates;

public sealed class UpdateCertificateEntryCommandValidator : AbstractValidator<UpdateCertificateEntryCommand>
{
    public UpdateCertificateEntryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}