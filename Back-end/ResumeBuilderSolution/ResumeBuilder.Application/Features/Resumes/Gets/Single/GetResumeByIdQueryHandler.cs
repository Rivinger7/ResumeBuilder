using MediatR;
using ResumeBuilder.Domain.Exceptions;
using ResumeBuilder.Domain.Interfaces.Repositories;
using ResumeBuilder.Domain.Models.Resumes;

namespace ResumeBuilder.Application.Features.Resumes.Gets.Single;

public sealed class GetResumeByIdQueryHandler(IResumeRepository resumeRepository) : IRequestHandler<GetResumeByIdQuery, ResumeInternalResponse>
{
    private readonly IResumeRepository _resumeRepository = resumeRepository;

    public async Task<ResumeInternalResponse> Handle(GetResumeByIdQuery request, CancellationToken cancellationToken)
    {
        return await _resumeRepository.GetByIdWithSectionsAsync(request.ResumeId, request.UserId, cancellationToken)
            ?? throw new NotFoundException("Not found resume");
    }
}
