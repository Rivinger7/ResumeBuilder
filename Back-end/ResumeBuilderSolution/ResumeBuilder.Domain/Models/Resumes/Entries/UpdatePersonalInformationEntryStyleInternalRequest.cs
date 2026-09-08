using ResumeBuilder.Domain.Enums;

namespace ResumeBuilder.Domain.Models.Resumes.Entries;

public sealed record UpdatePersonalInformationEntryStyleInternalRequest(Guid Id, TextAlignmentType? PersonalInformationTextAlignment, LayoutType? PersonalInformationLayout, MarkerStyleType? PersonalInformationMarkerStyle, IconStyleType? PersonalInformationIconStyle, bool? IsTitleProfessionalTitleOnSameLine);
