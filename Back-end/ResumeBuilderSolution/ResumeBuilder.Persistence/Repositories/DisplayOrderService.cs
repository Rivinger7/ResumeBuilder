using Microsoft.EntityFrameworkCore;
using ResumeBuilder.Domain.Exceptions;
using ResumeBuilder.Domain.Interfaces.Persistence;
using ResumeBuilder.Domain.Interfaces.Services;
using ResumeBuilder.Persistence.Context;

namespace ResumeBuilder.Persistence.Repositories;

internal sealed class DisplayOrderService(ApplicationDbContext context) : IDisplayOrderService
{
    private readonly ApplicationDbContext _context = context;

    public async Task BulkUpdateDisplayOrderEntriesByResumeSectionIdAsync<TEntry>(Guid resumeSectionId, Guid Id, int newDisplayOrder, CancellationToken cancellationToken = default) where TEntry : class, IDisplayOrderedEntry
    {
        int countDisplayOrder = await _context.Set<TEntry>().CountAsync(x => x.ResumeSectionId == resumeSectionId, cancellationToken: cancellationToken);
        if (newDisplayOrder > countDisplayOrder)
        {
            throw new BadRequestException($"New display order must be less than {countDisplayOrder}");
        }

        int from = _context.Set<TEntry>().First(x => x.Id == Id).DisplayOrder;
        int to = newDisplayOrder;
        if (from < to)
        {
            await _context.Set<TEntry>()
                .Where(x => x.ResumeSectionId == resumeSectionId &&
                            x.DisplayOrder > from &&
                            x.DisplayOrder <= to)
                .ExecuteUpdateAsync(s =>
                    s.SetProperty(x => x.DisplayOrder, x => x.DisplayOrder - 1), cancellationToken: cancellationToken);

            await _context.Set<TEntry>()
                .Where(x => x.Id == Id)
                .ExecuteUpdateAsync(s =>
                    s.SetProperty(x => x.DisplayOrder, to), cancellationToken: cancellationToken);
        }
        else
        {
            await _context.Set<TEntry>()
                .Where(x => x.ResumeSectionId == resumeSectionId &&
                            x.DisplayOrder >= to &&
                            x.DisplayOrder < from)
                .ExecuteUpdateAsync(s =>
                    s.SetProperty(x => x.DisplayOrder, x => x.DisplayOrder + 1), cancellationToken: cancellationToken);

            await _context.Set<TEntry>()
                .Where(x => x.Id == Id)
                .ExecuteUpdateAsync(s =>
                    s.SetProperty(x => x.DisplayOrder, to), cancellationToken: cancellationToken);
        }
    }

    public async Task BulkUpdateDisplayOrderSectionsByResumeIdAsync<TEntry>(Guid resumeId, Guid Id, int newDisplayOrder, CancellationToken cancellationToken = default) where TEntry : class, IDisplayOrderedResumeSection
    {
        int countDisplayOrder = await _context.Set<TEntry>().CountAsync(x => x.ResumeId == resumeId, cancellationToken: cancellationToken);
        if (newDisplayOrder > countDisplayOrder)
        {
            throw new BadRequestException($"New display order must be less than {countDisplayOrder}");
        }

        int from = _context.Set<TEntry>().First(x => x.Id == Id).DisplayOrder;
        int to = newDisplayOrder;
        if (from < to)
        {
            await _context.Set<TEntry>()
                .Where(x => x.ResumeId == resumeId &&
                            x.DisplayOrder > from &&
                            x.DisplayOrder <= to)
                .ExecuteUpdateAsync(s =>
                    s.SetProperty(x => x.DisplayOrder, x => x.DisplayOrder - 1), cancellationToken: cancellationToken);

            await _context.Set<TEntry>()
                .Where(x => x.Id == Id)
                .ExecuteUpdateAsync(s =>
                    s.SetProperty(x => x.DisplayOrder, to), cancellationToken: cancellationToken);
        }
        else
        {
            await _context.Set<TEntry>()
                .Where(x => x.ResumeId == resumeId &&
                            x.DisplayOrder >= to &&
                            x.DisplayOrder < from)
                .ExecuteUpdateAsync(s =>
                    s.SetProperty(x => x.DisplayOrder, x => x.DisplayOrder + 1), cancellationToken: cancellationToken);

            await _context.Set<TEntry>()
                .Where(x => x.Id == Id)
                .ExecuteUpdateAsync(s =>
                    s.SetProperty(x => x.DisplayOrder, to), cancellationToken: cancellationToken);
        }
    }
}
