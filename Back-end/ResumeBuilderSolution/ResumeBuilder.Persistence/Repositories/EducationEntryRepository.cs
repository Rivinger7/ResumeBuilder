using Microsoft.EntityFrameworkCore;
using ResumeBuilder.Domain.Entities.Resumes.Entries;
using ResumeBuilder.Domain.Interfaces.Repositories;
using ResumeBuilder.Domain.Models.Resumes.Entries;
using ResumeBuilder.Persistence.Context;

namespace ResumeBuilder.Persistence.Repositories;

internal class EducationEntryRepository(ApplicationDbContext context) : Repository<EducationEntry>(context), IEducationEntryRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<int?> GetCurrentDisplayOrderByResumeSectionIdAsync(Guid resumeSectionId, CancellationToken cancellationToken = default)
    {
        return await _context.EducationEntries
            .Where(x => x.ResumeSectionId == resumeSectionId)
            .MaxAsync(x => (int?)x.DisplayOrder, cancellationToken);
    }

    public async Task<int> UpdateContentByIdAsync(UpdateEducationEntryInternalRequest updateEducationEntryInternalRequest, CancellationToken cancellationToken = default)
    {
        return await _context.EducationEntries
            .Where(x => x.Id == updateEducationEntryInternalRequest.Id)
            .ExecuteUpdateAsync(setters =>
            {
                if (updateEducationEntryInternalRequest.SchoolName.IsSpecified)
                {
                    setters.SetProperty(x => x.SchoolName, updateEducationEntryInternalRequest.SchoolName.Value);
                }

                if (updateEducationEntryInternalRequest.Degree.IsSpecified)
                {
                    setters.SetProperty(x => x.Degree, updateEducationEntryInternalRequest.Degree.Value);
                }

                if (updateEducationEntryInternalRequest.Major.IsSpecified)
                {
                    setters.SetProperty(x => x.Major, updateEducationEntryInternalRequest.Major.Value);
                }

                if (updateEducationEntryInternalRequest.GPA.IsSpecified)
                {
                    setters.SetProperty(x => x.GPA, updateEducationEntryInternalRequest.GPA.Value);
                }

                if (updateEducationEntryInternalRequest.IsCurrent.IsSpecified)
                {
                    setters.SetProperty(x => x.IsCurrent, updateEducationEntryInternalRequest.IsCurrent.Value);
                }

                if (updateEducationEntryInternalRequest.StartDate.IsSpecified)
                {
                    setters.SetProperty(x => x.StartDate, updateEducationEntryInternalRequest.StartDate.Value);
                }

                if (updateEducationEntryInternalRequest.EndDate.IsSpecified)
                {
                    setters.SetProperty(x => x.EndDate, updateEducationEntryInternalRequest.EndDate.Value);
                }

                if (updateEducationEntryInternalRequest.Location.IsSpecified)
                {
                    setters.SetProperty(x => x.Location, updateEducationEntryInternalRequest.Location.Value);
                }

                if (updateEducationEntryInternalRequest.Description.IsSpecified)
                {
                    setters.SetProperty(x => x.Description, updateEducationEntryInternalRequest.Description.Value);
                }

                if (updateEducationEntryInternalRequest.IsByOrder.IsSpecified)
                {
                    setters.SetProperty(x => x.IsByOrder, updateEducationEntryInternalRequest.IsByOrder.Value);
                }
            }, cancellationToken);
    }
}
