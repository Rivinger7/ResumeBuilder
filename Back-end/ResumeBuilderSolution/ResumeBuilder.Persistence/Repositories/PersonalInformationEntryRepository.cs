using Microsoft.EntityFrameworkCore;
using ResumeBuilder.Domain.Entities.Resumes.Entries;
using ResumeBuilder.Domain.Interfaces.Repositories;
using ResumeBuilder.Domain.Models.Resumes.Entries;
using ResumeBuilder.Persistence.Context;

namespace ResumeBuilder.Persistence.Repositories;

internal class PersonalInformationEntryRepository(ApplicationDbContext context) : Repository<PersonalInformationEntry>(context), IPersonalInformationEntryRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<int> UpdateContentByIdAsync(UpdatePersonalInformationEntryInternalRequest updatePersonalInformationEntryInternalRequest, CancellationToken cancellationToken = default)
    {
        return await _context.PersonalInformationEntries
            .Where(x => x.Id == updatePersonalInformationEntryInternalRequest.Id)
            .ExecuteUpdateAsync(setters =>
            {
                if (updatePersonalInformationEntryInternalRequest.FullName.IsSpecified)
                {
                    setters.SetProperty(x => x.FullName, updatePersonalInformationEntryInternalRequest.FullName.Value);
                }

                if (updatePersonalInformationEntryInternalRequest.ProfessionalTitle.IsSpecified)
                {
                    setters.SetProperty(x => x.ProfessionalTitle, updatePersonalInformationEntryInternalRequest.ProfessionalTitle.Value);
                }

                if (updatePersonalInformationEntryInternalRequest.Email.IsSpecified)
                {
                    setters.SetProperty(x => x.Email, updatePersonalInformationEntryInternalRequest.Email.Value);
                }

                if (updatePersonalInformationEntryInternalRequest.PhoneNumber.IsSpecified)
                {
                    setters.SetProperty(x => x.PhoneNumber, updatePersonalInformationEntryInternalRequest.PhoneNumber.Value);
                }

                if (updatePersonalInformationEntryInternalRequest.Address.IsSpecified)
                {
                    setters.SetProperty(x => x.Address, updatePersonalInformationEntryInternalRequest.Address.Value);
                }

                if (updatePersonalInformationEntryInternalRequest.Website.IsSpecified)
                {
                    setters.SetProperty(x => x.Website, updatePersonalInformationEntryInternalRequest.Website.Value);
                }

                if (updatePersonalInformationEntryInternalRequest.LinkedIn.IsSpecified)
                {
                    setters.SetProperty(x => x.LinkedIn, updatePersonalInformationEntryInternalRequest.LinkedIn.Value);
                }

                if (updatePersonalInformationEntryInternalRequest.GitHub.IsSpecified)
                {
                    setters.SetProperty(x => x.GitHub, updatePersonalInformationEntryInternalRequest.GitHub.Value);
                }

                if (updatePersonalInformationEntryInternalRequest.PhotoUrl.IsSpecified)
                {
                    setters.SetProperty(x => x.PhotoUrl, updatePersonalInformationEntryInternalRequest.PhotoUrl.Value);
                }
            }, cancellationToken);
    }

    public async Task<int> UpdateStyleByIdAsync(UpdatePersonalInformationEntryStyleInternalRequest updatePersonalInformationEntryStyleInternalRequest, CancellationToken cancellationToken = default)
    {
        return await _context.PersonalInformationEntries
            .Where(x => x.Id == updatePersonalInformationEntryStyleInternalRequest.Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(x => x.PersonalInformationLayout, x => updatePersonalInformationEntryStyleInternalRequest.PersonalInformationLayout ?? x.PersonalInformationLayout)
                .SetProperty(x => x.PersonalInformationTextAlignment, x => updatePersonalInformationEntryStyleInternalRequest.PersonalInformationTextAlignment ?? x.PersonalInformationTextAlignment)
                .SetProperty(x => x.PersonalInformationMarkerStyle, x => updatePersonalInformationEntryStyleInternalRequest.PersonalInformationMarkerStyle ?? x.PersonalInformationMarkerStyle)
                .SetProperty(x => x.PersonalInformationIconStyle, x => updatePersonalInformationEntryStyleInternalRequest.PersonalInformationIconStyle ?? x.PersonalInformationIconStyle)
                .SetProperty(x => x.IsTitleProfessionalTitleOnSameLine, x => updatePersonalInformationEntryStyleInternalRequest.IsTitleProfessionalTitleOnSameLine ?? x.IsTitleProfessionalTitleOnSameLine),
                cancellationToken);
    }
}
