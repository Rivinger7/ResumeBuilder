using Microsoft.EntityFrameworkCore;
using ResumeBuilder.Domain.Entities.Resumes.Entries;
using ResumeBuilder.Domain.Interfaces.Repositories;
using ResumeBuilder.Domain.Models.Resumes.Entries;
using ResumeBuilder.Persistence.Context;

namespace ResumeBuilder.Persistence.Repositories;

internal class ProjectEntryRepository(ApplicationDbContext context) : Repository<ProjectEntry>(context), IProjectEntryRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<int?> GetCurrentDisplayOrderByResumeSectionIdAsync(Guid resumeSectionId, CancellationToken cancellationToken = default)
    {
        return await _context.ProjectEntries
            .Where(x => x.ResumeSectionId == resumeSectionId)
            .MaxAsync(x => (int?)x.DisplayOrder, cancellationToken);
    }

    public async Task<int> UpdateContentByIdAsync(UpdateProjectEntryInternalRequest updateProjectEntryInternalRequest, CancellationToken cancellationToken = default)
    {
        return await _context.ProjectEntries
            .Where(x => x.Id == updateProjectEntryInternalRequest.Id)
            .ExecuteUpdateAsync(setters =>
            {
                if (updateProjectEntryInternalRequest.Title.IsSpecified)
                {
                    setters.SetProperty(x => x.Title, updateProjectEntryInternalRequest.Title.Value);
                }

                if (updateProjectEntryInternalRequest.SubTitle.IsSpecified)
                {
                    setters.SetProperty(x => x.SubTitle, updateProjectEntryInternalRequest.SubTitle.Value);
                }

                if (updateProjectEntryInternalRequest.StartDate.IsSpecified)
                {
                    setters.SetProperty(x => x.StartDate, updateProjectEntryInternalRequest.StartDate.Value);
                }

                if (updateProjectEntryInternalRequest.EndDate.IsSpecified)
                {
                    setters.SetProperty(x => x.EndDate, updateProjectEntryInternalRequest.EndDate.Value);
                }

                if (updateProjectEntryInternalRequest.ProjectUrl.IsSpecified)
                {
                    setters.SetProperty(x => x.ProjectUrl, updateProjectEntryInternalRequest.ProjectUrl.Value);
                }

                if (updateProjectEntryInternalRequest.RepositoryUrl.IsSpecified)
                {
                    setters.SetProperty(x => x.RepositoryUrl, updateProjectEntryInternalRequest.RepositoryUrl.Value);
                }

                if (updateProjectEntryInternalRequest.Description.IsSpecified)
                {
                    setters.SetProperty(x => x.Description, updateProjectEntryInternalRequest.Description.Value);
                }
            }, cancellationToken);
    }
}
