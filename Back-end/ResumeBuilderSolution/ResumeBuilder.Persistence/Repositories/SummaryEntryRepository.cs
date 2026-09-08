using Microsoft.EntityFrameworkCore;
using ResumeBuilder.Domain.Entities.Resumes.Entries;
using ResumeBuilder.Domain.Interfaces.Repositories;
using ResumeBuilder.Domain.Models.Resumes.Entries;
using ResumeBuilder.Persistence.Context;

namespace ResumeBuilder.Persistence.Repositories;

internal class SummaryEntryRepository(ApplicationDbContext context) : Repository<SummaryEntry>(context), ISummaryEntryRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<int?> GetCurrentDisplayOrderByResumeSectionIdAsync(Guid resumeSectionId, CancellationToken cancellationToken = default)
    {
        return await _context.SummaryEntries
            .Where(x => x.ResumeSectionId == resumeSectionId)
            .MaxAsync(x => (int?)x.DisplayOrder, cancellationToken);
    }

    public async Task<int> UpdateContentByIdAsync(UpdateSummaryEntryInternalRequest updateSummaryEntryInternalRequest, CancellationToken cancellationToken = default)
    {
        return await _context.SummaryEntries
            .Where(x => x.Id == updateSummaryEntryInternalRequest.Id)
            .ExecuteUpdateAsync(setters =>
            {
                if (updateSummaryEntryInternalRequest.Summary.IsSpecified)
                {
                    setters.SetProperty(x => x.Summary, updateSummaryEntryInternalRequest.Summary.Value);
                }
            }, cancellationToken);
    }
}
