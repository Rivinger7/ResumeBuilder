using ResumeBuilder.Domain.Common;

namespace ResumeBuilder.Api.Models.Entries;

public sealed record UpdateObjectiveEntryRequest(Optional<string?> Title, Optional<string?> SubTitle, Optional<string?> Description);
