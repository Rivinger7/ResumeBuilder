using ResumeBuilder.Domain.Enums;

namespace ResumeBuilder.Api.Models.Reorders.Entries;

public sealed record UpdateDisplayOrderEntryRequest(ResumeSectionType Type, int NewDisplayOrder);
