using MediatR;
using ResumeBuilder.Domain.Enums;

namespace ResumeBuilder.Application.Features.Entries.PersonalInformations.Updates;

public sealed record UpdatePersonalInformationEntryStyleCommand(Guid Id, TextAlignmentType? PersonalInformationTextAlignment, LayoutType? PersonalInformationLayout, MarkerStyleType? PersonalInformationMarkerStyle, IconStyleType? PersonalInformationIconStyle, bool? IsTitleProfessionalTitleOnSameLine) : IRequest<UpdatePersonalInformationEntryStyleResponse>;