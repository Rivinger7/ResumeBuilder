using Microsoft.EntityFrameworkCore;
using ResumeBuilder.Domain.Entities.Resumes.Entries;
using ResumeBuilder.Domain.Enums;
using ResumeBuilder.Domain.Interfaces.Repositories;
using ResumeBuilder.Domain.Models.Resumes.Entries;
using ResumeBuilder.Persistence.Context;

namespace ResumeBuilder.Persistence.Repositories;

internal class CertificateEntryRepository(ApplicationDbContext context) : Repository<CertificateEntry>(context), ICertificateEntryRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<int?> GetCurrentDisplayOrderByResumeSectionIdAsync(Guid resumeSectionId, CancellationToken cancellationToken = default)
    {
        return await _context.CertificateEntries
            .Where(x => x.ResumeSectionId == resumeSectionId)
            .MaxAsync(x => (int?)x.DisplayOrder, cancellationToken);
    }

    public async Task<CertificateEntry?> GetFirstByResumeSectionIdAsync(Guid resumeSectionId, CancellationToken cancellationToken = default)
    {
        return await _context.CertificateEntries
            .Where(x => x.ResumeSectionId == resumeSectionId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<int> UpdateContentByIdAsync(UpdateCertificateEntryInternalRequest updateCertificateEntryInternalRequest, CancellationToken cancellationToken = default)
    {
        return await _context.CertificateEntries
            .Where(x => x.Id == updateCertificateEntryInternalRequest.Id)
            .ExecuteUpdateAsync(setters =>
            {
                if (updateCertificateEntryInternalRequest.Title.IsSpecified)
                {
                    setters.SetProperty(x => x.Title, updateCertificateEntryInternalRequest.Title.Value);
                }

                if (updateCertificateEntryInternalRequest.CertificateUrl.IsSpecified)
                {
                    setters.SetProperty(x => x.CertificateUrl, updateCertificateEntryInternalRequest.CertificateUrl.Value);
                }

                if (updateCertificateEntryInternalRequest.Description.IsSpecified)
                {
                    setters.SetProperty(x => x.Description, updateCertificateEntryInternalRequest.Description.Value);
                }
            }, cancellationToken);
    }

    public async Task<int> UpdateStyleByIdAsync(UpdateCertificateEntryStyleInternalRequest updateCertificateEntryStyleInternalRequest, CancellationToken cancellationToken = default)
    {
        Guid resumeSectionId = await _context.CertificateEntries
            .Where(x => x.Id == updateCertificateEntryStyleInternalRequest.Id)
            .Select(x => x.ResumeSectionId)
            .FirstOrDefaultAsync(cancellationToken);

        // Determine if layout is being updated and its value
        bool isLayoutSpecified = updateCertificateEntryStyleInternalRequest.CertificateLayout.HasValue;
        LayoutType? newLayout = updateCertificateEntryStyleInternalRequest.CertificateLayout;

        return await _context.CertificateEntries
            .Where(x => x.ResumeSectionId == resumeSectionId)
            .ExecuteUpdateAsync(setters =>
            {
                if (isLayoutSpecified)
                {
                    setters.SetProperty(x => x.CertificateLayout, newLayout!.Value);

                    // If switching to Grid, clear row-based fields
                    if (newLayout == LayoutType.Grid)
                    {
                        // clear row-related values
                        setters.SetProperty(x => x.CertificateRowSpacing, (string?)null);
                        setters.SetProperty(x => x.IsStartRowsWithBullet, false);
                    }

                    // If switching to Rows, clear grid-specific value
                    if (newLayout == LayoutType.Rows)
                    {
                        setters.SetProperty(x => x.CertificateGridColumn, (int?)null);
                    }
                }

                if (updateCertificateEntryStyleInternalRequest.CertificateGridColumn.HasValue && newLayout != LayoutType.Rows)
                {
                    setters.SetProperty(x => x.CertificateGridColumn, updateCertificateEntryStyleInternalRequest.CertificateGridColumn.Value);
                }

                if (updateCertificateEntryStyleInternalRequest.CertificateRowSpacing is not null && newLayout != LayoutType.Grid)
                {
                    setters.SetProperty(
                        x => x.CertificateRowSpacing,
                        updateCertificateEntryStyleInternalRequest.CertificateRowSpacing);
                }

                if (updateCertificateEntryStyleInternalRequest.IsStartRowsWithBullet.HasValue && newLayout != LayoutType.Grid)
                {
                    setters.SetProperty(
                        x => x.IsStartRowsWithBullet,
                        updateCertificateEntryStyleInternalRequest.IsStartRowsWithBullet.Value);
                }

                if (updateCertificateEntryStyleInternalRequest.SubinfoStyle is not null)
                {
                    setters.SetProperty(x => x.SubinfoStyle, updateCertificateEntryStyleInternalRequest.SubinfoStyle);
                }
            }, cancellationToken);
    }
}
