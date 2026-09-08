using Mapster;
using Microsoft.EntityFrameworkCore;
using ResumeBuilder.Domain.Common;
using ResumeBuilder.Domain.Entities.Resumes;
using ResumeBuilder.Domain.Helpers;
using ResumeBuilder.Domain.Interfaces.Repositories;
using ResumeBuilder.Domain.Models.Resumes;
using ResumeBuilder.Persistence.Context;

namespace ResumeBuilder.Persistence.Repositories;

internal sealed class ResumeRepository(ApplicationDbContext context) : Repository<Resume>(context), IResumeRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<PagedResult<ResumeInternalResponse>> GetPagedByUserIdAsync(Guid userId, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        IQueryable<ResumeInternalResponse> query = _context.Resumes
        .AsNoTracking()
        .AsSplitQuery()
        .Where(x => x.UserId == userId)
        .OrderByDescending(x => x.UpdatedAt)
        .ThenByDescending(x => x.Id)
        .ProjectToType<ResumeInternalResponse>();

        int totalItems = await query.CountAsync(cancellationToken);

        List<ResumeInternalResponse> items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<ResumeInternalResponse>(items, totalItems);
    }

    public async Task<bool> IsResumeExistedByTitle(string title, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Resumes
            .AnyAsync(x => x.Title == title && x.UserId == userId, cancellationToken);
    }

    public async Task<ResumeInternalResponse?> GetByIdWithSectionsAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Resumes
            .AsNoTracking()
            .AsSplitQuery()
            .Where(x => x.Id == id && x.UserId == userId)
            .ProjectToType<ResumeInternalResponse>()
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task UpdateThumbnailUrlAsync(Guid id, string thumbnailUrl, CancellationToken cancellationToken = default)
    {
        await _context.Resumes
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.ThumbnailUrl, thumbnailUrl)
                .SetProperty(x => x.UpdatedAt, CustomTimeProvider.UtcNowOffset),
                cancellationToken);
    }
}
