using MediatR;
using ResumeBuilder.Domain.Entities.Resumes.Entries;
using ResumeBuilder.Domain.Interfaces.Persistence;
using ResumeBuilder.Domain.Interfaces.Repositories;

namespace ResumeBuilder.Application.Features.Entries.Languages.Creates;

public sealed class CreateLanguageEntryCommandHandler(ILanguageEntryRepository languageEntryRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateLanguageEntryCommand, CreateLanguageEntryResponse>
{
    private readonly ILanguageEntryRepository _languageEntryRepository = languageEntryRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<CreateLanguageEntryResponse> Handle(CreateLanguageEntryCommand request, CancellationToken cancellationToken)
    {
        int currentDisplayOrder = await _languageEntryRepository.GetCurrentDisplayOrderByResumeSectionIdAsync(request.ResumeSectionId, cancellationToken) ?? 0;

        LanguageEntry languageEntry = new()
        {
            Id = Guid.NewGuid(),
            ResumeSectionId = request.ResumeSectionId,
            LanguageName = request.LanguageName,
            Proficiency = request.Proficiency,
            DisplayOrder = currentDisplayOrder + 1,
            LanguageLayout = request.LanguageLayout,
            LanguageGridColumn = request.LanguageGridColumn,
            LanguageRowSpacing = request.LanguageRowSpacing,
            IsStartRowsWithBullet = request.IsStartRowsWithBullet,
            SubinfoStyle = request.SubinfoStyle
        };

        await _languageEntryRepository.AddAsync(languageEntry, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateLanguageEntryResponse();

    }
}