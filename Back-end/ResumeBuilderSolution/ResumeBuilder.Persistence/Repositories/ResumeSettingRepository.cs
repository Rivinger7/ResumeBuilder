using ResumeBuilder.Domain.Entities.Resumes;
using ResumeBuilder.Domain.Interfaces.Repositories;
using ResumeBuilder.Persistence.Context;

namespace ResumeBuilder.Persistence.Repositories;

internal sealed class ResumeSettingRepository(ApplicationDbContext context) : Repository<ResumeSetting>(context), IResumeSettingRepository
{

}
