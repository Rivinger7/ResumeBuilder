using MediatR;
using ResumeBuilder.Domain.Exceptions;
using ResumeBuilder.Domain.Interfaces.Persistence;
using ResumeBuilder.Domain.Interfaces.Repositories;
using ResumeBuilder.Domain.Models.Resumes.Entries;

namespace ResumeBuilder.Application.Features.Entries.Languages.Updates;

public sealed class UpdateLanguageEntryCommandHandler(ILanguageEntryRepository languageEntryRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateLanguageEntryCommand, UpdateLanguageEntryResponse>
{
    private readonly ILanguageEntryRepository _languageEntryRepository = languageEntryRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<UpdateLanguageEntryResponse> Handle(UpdateLanguageEntryCommand request, CancellationToken cancellationToken)
    {
        UpdateLanguageEntryInternalRequest updateLanguageEntryInternalRequest = new(request.Id, request.LanguageName, request.Proficiency);

        int rowsAffected = await _languageEntryRepository.UpdateContentByIdAsync(updateLanguageEntryInternalRequest, cancellationToken);
        if (rowsAffected == 0)
        {
            throw new NotFoundException("Language entry not found");
        }

        return new UpdateLanguageEntryResponse();
    }
}