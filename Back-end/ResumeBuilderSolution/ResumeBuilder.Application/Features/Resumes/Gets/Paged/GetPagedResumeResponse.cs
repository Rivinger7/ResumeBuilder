using ResumeBuilder.Domain.Enums;
using ResumeBuilder.Domain.Models.Resumes;

namespace ResumeBuilder.Application.Features.Resumes.Gets.Paged;

public sealed record GetPagedResumeResponse(Guid Id, string Title, string ThumbnailUrl, string? Description, ResumeStatus Status, ResumeSettingInternalResponse Settings, IEnumerable<ResumeSectionInternalResponse>? ResumeSections);