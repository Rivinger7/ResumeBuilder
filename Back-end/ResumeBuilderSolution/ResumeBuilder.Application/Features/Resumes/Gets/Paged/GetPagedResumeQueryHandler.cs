using Mapster;
using MediatR;
using ResumeBuilder.Application.Models;
using ResumeBuilder.Domain.Common;
using ResumeBuilder.Domain.Interfaces.Repositories;
using ResumeBuilder.Domain.Models.Resumes;

namespace ResumeBuilder.Application.Features.Resumes.Gets.Paged;

public sealed class GetPagedResumeQueryHandler(IResumeRepository resumeRepository) : IRequestHandler<GetPagedResumeQuery, PagedResponse<GetPagedResumeResponse>>
{
    private readonly IResumeRepository _resumeRepository = resumeRepository;

    public async Task<PagedResponse<GetPagedResumeResponse>> Handle(GetPagedResumeQuery request, CancellationToken cancellationToken)
    {
        PagedResult<ResumeInternalResponse> result = await _resumeRepository.GetPagedByUserIdAsync(request.UserId, request.PageNumber, request.PageSize, cancellationToken);
        IReadOnlyCollection<GetPagedResumeResponse> items = result.Items.Adapt<IReadOnlyCollection<GetPagedResumeResponse>>();

        return PagedResponse<GetPagedResumeResponse>.Create(items, request.PageNumber, request.PageSize, result.TotalItems);
    }
}