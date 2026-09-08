using Microsoft.EntityFrameworkCore;
using ResumeBuilder.Domain.Entities.Resumes.Entries;
using ResumeBuilder.Domain.Interfaces.Repositories;
using ResumeBuilder.Domain.Models.Resumes.Entries;
using ResumeBuilder.Persistence.Context;

namespace ResumeBuilder.Persistence.Repositories;

internal class ExperienceEntryRepository(ApplicationDbContext context) : Repository<ExperienceEntry>(context), IExperienceEntryRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<int?> GetCurrentDisplayOrderByResumeSectionIdAsync(Guid resumeSectionId, CancellationToken cancellationToken = default)
    {
        return await _context.ExperienceEntries
            .Where(x => x.ResumeSectionId == resumeSectionId)
            .MaxAsync(x => (int?)x.DisplayOrder, cancellationToken);
    }

    public async Task<int> UpdateContentByIdAsync(UpdateExperienceEntryInternalRequest updateExperienceEntryInternalRequest, CancellationToken cancellationToken = default)
    {
        return await _context.ExperienceEntries
            .Where(x => x.Id == updateExperienceEntryInternalRequest.Id)
            .ExecuteUpdateAsync(setters =>
            {
                if (updateExperienceEntryInternalRequest.CompanyName.IsSpecified)
                {
                    setters.SetProperty(x => x.CompanyName, updateExperienceEntryInternalRequest.CompanyName.Value);
                }

                if (updateExperienceEntryInternalRequest.Position.IsSpecified)
                {
                    setters.SetProperty(x => x.Position, updateExperienceEntryInternalRequest.Position.Value);
                }

                if (updateExperienceEntryInternalRequest.StartDate.IsSpecified)
                {
                    setters.SetProperty(x => x.StartDate, updateExperienceEntryInternalRequest.StartDate.Value);
                }

                if (updateExperienceEntryInternalRequest.EndDate.IsSpecified)
                {
                    setters.SetProperty(x => x.EndDate, updateExperienceEntryInternalRequest.EndDate.Value);
                }

                if (updateExperienceEntryInternalRequest.IsCurrent.IsSpecified)
                {
                    setters.SetProperty(x => x.IsCurrent, updateExperienceEntryInternalRequest.IsCurrent.Value);
                }

                if (updateExperienceEntryInternalRequest.Location.IsSpecified)
                {
                    setters.SetProperty(x => x.Location, updateExperienceEntryInternalRequest.Location.Value);
                }

                if (updateExperienceEntryInternalRequest.Description.IsSpecified)
                {
                    setters.SetProperty(x => x.Description, updateExperienceEntryInternalRequest.Description.Value);
                }

                if (updateExperienceEntryInternalRequest.IsByOrder.IsSpecified)
                {
                    setters.SetProperty(x => x.IsByOrder, updateExperienceEntryInternalRequest.IsByOrder.Value);
                }
            }, cancellationToken);
    }
}
