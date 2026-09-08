using MediatR;
using ResumeBuilder.Domain.Exceptions;
using ResumeBuilder.Domain.Interfaces.Persistence;
using ResumeBuilder.Domain.Interfaces.Repositories;
using ResumeBuilder.Domain.Models.Resumes.Entries;

namespace ResumeBuilder.Application.Features.Entries.Languages.Updates;

public sealed class UpdateLanguageEntryStyleCommandHandler(ILanguageEntryRepository languageEntryRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateLanguageEntryStyleCommand, UpdateLanguageEntryStyleResponse>
{
    private readonly ILanguageEntryRepository _languageEntryRepository = languageEntryRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<UpdateLanguageEntryStyleResponse> Handle(UpdateLanguageEntryStyleCommand request, CancellationToken cancellationToken)
    {
        UpdateLanguageEntryStyleInternalRequest updateLanguageEntryStyleInternalRequest = new(request.Id, request.LanguageLayout, request.LanguageGridColumn, request.LanguageRowSpacing, request.IsStartRowsWithBullet, request.SubinfoStyle);

        int rowsAffected = await _languageEntryRepository.UpdateStyleByIdAsync(updateLanguageEntryStyleInternalRequest, cancellationToken);
        if (rowsAffected == 0)
        {
            throw new NotFoundException("Language entry not found");
        }

        return new UpdateLanguageEntryStyleResponse();
    }
}
