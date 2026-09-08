using ResumeBuilder.Domain.Enums;

namespace ResumeBuilder.Domain.Models.Resumes;

public sealed record ResumeInternalResponse(Guid Id, string Title, string? Description, ResumeStatus Status, string? ThumbnailUrl, ResumeSettingInternalResponse Settings, IEnumerable<ResumeSectionInternalResponse>? ResumeSections);
