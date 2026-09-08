using ResumeBuilder.Domain.Enums;

namespace ResumeBuilder.Api.Models.Entries;

public sealed record UpdatePersonalInformationEntryStyleRequest(TextAlignmentType? PersonalInformationTextAlignment, LayoutType? PersonalInformationLayout, MarkerStyleType? PersonalInformationMarkerStyle, IconStyleType? PersonalInformationIconStyle, bool? IsTitleProfessionalTitleOnSameLine);
