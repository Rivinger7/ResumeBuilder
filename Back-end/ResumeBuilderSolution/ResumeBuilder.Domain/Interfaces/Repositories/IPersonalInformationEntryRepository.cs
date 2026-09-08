using ResumeBuilder.Domain.Entities.Resumes.Entries;
using ResumeBuilder.Domain.Interfaces.Persistence;
using ResumeBuilder.Domain.Models.Resumes.Entries;

namespace ResumeBuilder.Domain.Interfaces.Repositories;

public interface IPersonalInformationEntryRepository : IRepository<PersonalInformationEntry>
{
    Task<int> UpdateContentByIdAsync(UpdatePersonalInformationEntryInternalRequest updatePersonalInformationEntryInternalRequest, CancellationToken cancellationToken = default);
    Task<int> UpdateStyleByIdAsync(UpdatePersonalInformationEntryStyleInternalRequest updatePersonalInformationEntryStyleInternalRequest, CancellationToken cancellationToken = default);
}
