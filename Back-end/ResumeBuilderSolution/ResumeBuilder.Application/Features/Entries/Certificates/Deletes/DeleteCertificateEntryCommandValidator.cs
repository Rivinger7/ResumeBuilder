using FluentValidation;

namespace ResumeBuilder.Application.Features.Entries.Certificates.Deletes;

public sealed class DeleteCertificateEntryCommandValidator : AbstractValidator<DeleteCertificateEntryCommand>
{
    public DeleteCertificateEntryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}