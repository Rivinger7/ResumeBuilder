using Microsoft.EntityFrameworkCore;
using ResumeBuilder.Domain.Entities.Resumes.Entries;
using ResumeBuilder.Domain.Interfaces.Repositories;
using ResumeBuilder.Domain.Models.Resumes.Entries;
using ResumeBuilder.Persistence.Context;

namespace ResumeBuilder.Persistence.Repositories;

internal class LanguageEntryRepository(ApplicationDbContext context) : Repository<LanguageEntry>(context), ILanguageEntryRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<int?> GetCurrentDisplayOrderByResumeSectionIdAsync(Guid resumeSectionId, CancellationToken cancellationToken = default)
    {
        return await _context.LanguageEntries
            .Where(x => x.ResumeSectionId == resumeSectionId)
            .MaxAsync(x => (int?)x.DisplayOrder, cancellationToken);
    }

    public async Task<int> UpdateContentByIdAsync(UpdateLanguageEntryInternalRequest updateLanguageEntryInternalRequest, CancellationToken cancellationToken = default)
    {
        return await _context.LanguageEntries
            .Where(x => x.Id == updateLanguageEntryInternalRequest.Id)
            .ExecuteUpdateAsync(setters =>
            {
                if (updateLanguageEntryInternalRequest.LanguageName.IsSpecified)
                {
                    setters.SetProperty(x => x.LanguageName, updateLanguageEntryInternalRequest.LanguageName.Value);
                }

                if (updateLanguageEntryInternalRequest.Proficiency.IsSpecified)
                {
                    setters.SetProperty(x => x.Proficiency, updateLanguageEntryInternalRequest.Proficiency.Value);
                }
            }, cancellationToken);
    }

    public async Task<int> UpdateStyleByIdAsync(UpdateLanguageEntryStyleInternalRequest updateLanguageEntryStyleInternalRequest, CancellationToken cancellationToken = default)
    {
        return await _context.LanguageEntries
            .Where(x => x.Id == updateLanguageEntryStyleInternalRequest.Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(x => x.LanguageLayout, x => updateLanguageEntryStyleInternalRequest.LanguageLayout ?? x.LanguageLayout)
                .SetProperty(x => x.LanguageGridColumn, x => updateLanguageEntryStyleInternalRequest.LanguageGridColumn ?? x.LanguageGridColumn)
                .SetProperty(x => x.LanguageRowSpacing, x => updateLanguageEntryStyleInternalRequest.LanguageRowSpacing ?? x.LanguageRowSpacing)
                .SetProperty(x => x.IsStartRowsWithBullet, x => updateLanguageEntryStyleInternalRequest.IsStartRowsWithBullet ?? x.IsStartRowsWithBullet)
                .SetProperty(x => x.SubinfoStyle, x => updateLanguageEntryStyleInternalRequest.SubinfoStyle ?? x.SubinfoStyle),
                cancellationToken);
    }
}
