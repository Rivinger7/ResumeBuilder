namespace ResumeBuilder.Domain.Models.Resumes.Entries;

public sealed record SummaryEntryInternalResponse(Guid Id, string? Summary, int DisplayOrder);
