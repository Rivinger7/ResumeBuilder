using Microsoft.EntityFrameworkCore;
using ResumeBuilder.Domain.Entities.Resumes.Entries;
using ResumeBuilder.Domain.Interfaces.Repositories;
using ResumeBuilder.Domain.Models.Resumes.Entries;
using ResumeBuilder.Persistence.Context;

namespace ResumeBuilder.Persistence.Repositories;

internal class SkillEntryRepository(ApplicationDbContext context) : Repository<SkillEntry>(context), ISkillEntryRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<int?> GetCurrentDisplayOrderByResumeSectionIdAsync(Guid resumeSectionId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<SkillEntry>()
            .Where(x => x.ResumeSectionId == resumeSectionId)
            .MaxAsync(x => (int?)x.DisplayOrder, cancellationToken);
    }

    public async Task<int> UpdateContentByIdAsync(UpdateSkillEntryInternalRequest updateSkillEntryInternalRequest, CancellationToken cancellationToken = default)
    {
        return await _context.Set<SkillEntry>()
            .Where(x => x.Id == updateSkillEntryInternalRequest.Id)
            .ExecuteUpdateAsync(setters =>
            {
                if (updateSkillEntryInternalRequest.SkillName.IsSpecified)
                {
                    setters.SetProperty(x => x.SkillName, updateSkillEntryInternalRequest.SkillName.Value);
                }

                if (updateSkillEntryInternalRequest.Description.IsSpecified)
                {
                    setters.SetProperty(x => x.Description, updateSkillEntryInternalRequest.Description.Value);
                }

                if (updateSkillEntryInternalRequest.SkillLevel.IsSpecified)
                {
                    setters.SetProperty(x => x.SkillLevel, updateSkillEntryInternalRequest.SkillLevel.Value);
                }
            }, cancellationToken);
    }

    public async Task<int> UpdateStyleByIdAsync(UpdateSkillEntryStyleInternalRequest updateSkillEntryStyleInternalRequest, CancellationToken cancellationToken = default)
    {
        return await _context.Set<SkillEntry>()
            .Where(x => x.Id == updateSkillEntryStyleInternalRequest.Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(x => x.SkillLayout, x => updateSkillEntryStyleInternalRequest.SkillLayout ?? x.SkillLayout)
                .SetProperty(x => x.SkillGridColumn, x => updateSkillEntryStyleInternalRequest.SkillGridColumn ?? x.SkillGridColumn)
                .SetProperty(x => x.SkillRowSpacing, x => updateSkillEntryStyleInternalRequest.SkillRowSpacing ?? x.SkillRowSpacing)
                .SetProperty(x => x.IsStartRowsWithBullet, x => updateSkillEntryStyleInternalRequest.IsStartRowsWithBullet ?? x.IsStartRowsWithBullet)
                .SetProperty(x => x.SubinfoStyle, x => updateSkillEntryStyleInternalRequest.SubinfoStyle ?? x.SubinfoStyle),
                cancellationToken);
    }
}
