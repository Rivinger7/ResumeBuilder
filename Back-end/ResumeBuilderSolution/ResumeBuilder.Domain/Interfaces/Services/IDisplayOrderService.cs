using ResumeBuilder.Domain.Interfaces.Persistence;

namespace ResumeBuilder.Domain.Interfaces.Services;

public interface IDisplayOrderService
{
    Task BulkUpdateDisplayOrderSectionsByResumeIdAsync<TEntry>(Guid resumeId, Guid Id, int newDisplayOrder, CancellationToken cancellationToken = default) where TEntry : class, IDisplayOrderedResumeSection;
    Task BulkUpdateDisplayOrderEntriesByResumeSectionIdAsync<TEntry>(Guid resumeSectionId, Guid Id, int newDisplayOrder, CancellationToken cancellationToken = default) where TEntry : class, IDisplayOrderedEntry;
}
