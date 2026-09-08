using MediatR;
using ResumeBuilder.Domain.Entities.Resumes;
using ResumeBuilder.Domain.Enums;
using ResumeBuilder.Domain.Exceptions;
using ResumeBuilder.Domain.Interfaces.Repositories;

namespace ResumeBuilder.Application.Features.ResumeSections.Deletes;

public sealed class DeleteResumeSectionCommandHandler(IResumeSectionRepository resumeSectionRepository) : IRequestHandler<DeleteResumeSectionCommand, DeleteResumeSectionResponse>
{
    private readonly IResumeSectionRepository _resumeSectionRepository = resumeSectionRepository;

    public async Task<DeleteResumeSectionResponse> Handle(DeleteResumeSectionCommand request, CancellationToken cancellationToken)
    {
        ResumeSection? resumeSection = await _resumeSectionRepository.GetByIdAsync(request.Id, cancellationToken);
        if (resumeSection is null)
        {
            throw new NotFoundException($"Resume section with ID {request.Id} not found.");
        }

        if (resumeSection.Type == ResumeSectionType.PersonalInformation)
        {
            throw new ConflictException("Personal Information section cannot be deleted.");
        }

        await _resumeSectionRepository.ExecuteDeleteByIdAsync(request.Id, cancellationToken);

        return new DeleteResumeSectionResponse();
    }
}
