using Microsoft.EntityFrameworkCore;
using ResumeBuilder.Domain.Entities.Resumes;
using ResumeBuilder.Domain.Enums;
using ResumeBuilder.Domain.Interfaces.Repositories;
using ResumeBuilder.Persistence.Context;

namespace ResumeBuilder.Persistence.Repositories;

internal sealed class ResumeSectionRepository(ApplicationDbContext context) : Repository<ResumeSection>(context), IResumeSectionRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<bool> IsResumeSectionExistedByType(Guid resumeId, ResumeSectionType type, CancellationToken cancellationToken = default)
    {
        return await _context.ResumeSections
            .AnyAsync(x => x.ResumeId == resumeId && x.Type == type, cancellationToken);
    }
}
