using Microsoft.EntityFrameworkCore;
using ResumeBuilder.Domain.Entities.Resumes.Entries;
using ResumeBuilder.Domain.Interfaces.Repositories;
using ResumeBuilder.Domain.Models.Resumes.Entries;
using ResumeBuilder.Persistence.Context;

namespace ResumeBuilder.Persistence.Repositories;

internal class ObjectiveEntryRepository(ApplicationDbContext context) : Repository<ObjectiveEntry>(context), IObjectiveEntryRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<int?> GetCurrentDisplayOrderByResumeSectionIdAsync(Guid resumeSectionId, CancellationToken cancellationToken = default)
    {
        return await _context.ObjectiveEntries
            .Where(x => x.ResumeSectionId == resumeSectionId)
            .MaxAsync(x => (int?)x.DisplayOrder, cancellationToken);
    }

    public async Task<int> UpdateContentByIdAsync(UpdateObjectiveEntryInternalRequest updateObjectiveEntryInternalRequest, CancellationToken cancellationToken = default)
    {
        return await _context.ObjectiveEntries
            .Where(x => x.Id == updateObjectiveEntryInternalRequest.Id)
            .ExecuteUpdateAsync(setters =>
            {
                if (updateObjectiveEntryInternalRequest.Title.IsSpecified)
                {
                    setters.SetProperty(x => x.Title, updateObjectiveEntryInternalRequest.Title.Value);
                }

                if (updateObjectiveEntryInternalRequest.SubTitle.IsSpecified)
                {
                    setters.SetProperty(x => x.SubTitle, updateObjectiveEntryInternalRequest.SubTitle.Value);
                }

                if (updateObjectiveEntryInternalRequest.Description.IsSpecified)
                {
                    setters.SetProperty(x => x.Description, updateObjectiveEntryInternalRequest.Description.Value);
                }
            }, cancellationToken);
    }
}
