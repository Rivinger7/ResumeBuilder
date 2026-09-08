using FluentValidation;

namespace ResumeBuilder.Application.Features.Entries.PersonalInformations.Photos.Uploads;

public sealed class UploadPersonalInformationPhotoCommandValidator : AbstractValidator<UploadPersonalInformationPhotoCommand>
{
    private static readonly string[] AllowedExtensions = [".png", ".jpg", ".jpeg", ".webp"];

    public UploadPersonalInformationPhotoCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.FileExtension)
            .Must(extension => AllowedExtensions.Contains(extension.ToLowerInvariant()))
            .WithMessage("Only PNG, JPG, and WEBP images are allowed.");
    }
}
