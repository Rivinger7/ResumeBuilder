using MediatR;
using ResumeBuilder.Application.Models;

namespace ResumeBuilder.Application.Features.Resumes.Gets.Paged;

public sealed record GetPagedResumeQuery(Guid UserId, int PageNumber, int PageSize) : IRequest<PagedResponse<GetPagedResumeResponse>>;