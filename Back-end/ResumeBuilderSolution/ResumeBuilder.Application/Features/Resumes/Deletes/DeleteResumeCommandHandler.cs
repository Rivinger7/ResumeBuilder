using MediatR;
using ResumeBuilder.Domain.Exceptions;
using ResumeBuilder.Domain.Interfaces.Persistence;
using ResumeBuilder.Domain.Interfaces.Repositories;

namespace ResumeBuilder.Application.Features.Resumes.Deletes;

public sealed class DeleteResumeCommandHandler(IResumeRepository resumeRepository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteResumeCommand, DeleteResumeResponse>
{
    private readonly IResumeRepository _resumeRepository = resumeRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<DeleteResumeResponse> Handle(DeleteResumeCommand request, CancellationToken cancellationToken)
    {
        int rowsAffected = await _resumeRepository.ExecuteDeleteByIdAsync(request.Id, cancellationToken);
        if (rowsAffected == 0)
        {
            throw new NotFoundException("Cannot delete or not found");
        }

        return new DeleteResumeResponse();
    }
}