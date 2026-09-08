using MediatR;
using ResumeBuilder.Domain.Exceptions;
using ResumeBuilder.Domain.Interfaces.Repositories;

namespace ResumeBuilder.Application.Features.Entries.Languages.Deletes;

public sealed class DeleteLanguageEntryCommandHandler(ILanguageEntryRepository languageEntryRepository) : IRequestHandler<DeleteLanguageEntryCommand, DeleteLanguageEntryResponse>
{
    private readonly ILanguageEntryRepository _languageEntryRepository = languageEntryRepository;

    public async Task<DeleteLanguageEntryResponse> Handle(DeleteLanguageEntryCommand request, CancellationToken cancellationToken)
    {
        int rowsAffected = await _languageEntryRepository.ExecuteDeleteByIdAsync(request.Id, cancellationToken);
        if (rowsAffected == 0)
        {
            throw new NotFoundException($"Language entry with ID {request.Id} not found.");
        }

        return new DeleteLanguageEntryResponse();
    }
}
