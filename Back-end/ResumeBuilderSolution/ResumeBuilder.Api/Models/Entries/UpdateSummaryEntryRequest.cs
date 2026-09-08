using ResumeBuilder.Domain.Common;

namespace ResumeBuilder.Api.Models.Entries;

public sealed record UpdateSummaryEntryRequest(Optional<string?> Summary);
