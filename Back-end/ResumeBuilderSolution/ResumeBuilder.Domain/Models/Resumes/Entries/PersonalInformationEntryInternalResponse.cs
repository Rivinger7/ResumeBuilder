using ResumeBuilder.Domain.Enums;

namespace ResumeBuilder.Domain.Models.Resumes.Entries;

public sealed record PersonalInformationEntryInternalResponse(Guid Id, string FullName, string ProfessionalTitle, string? Email, string? PhoneNumber, string? Address, string? Website, string? Linkedin, string? Github, string? PhotoUrl, TextAlignmentType PersonalInformationTextAlignment, LayoutType PersonalInformationLayout, string? PersonalInformationMarkerStyle, string? PersonalInformationIconStyle, bool IsTitleProfessionalTitleOnSameLine);
