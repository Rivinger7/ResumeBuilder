using MediatR;
using ResumeBuilder.Domain.Models.Resumes;

namespace ResumeBuilder.Application.Features.Resumes.Gets.Single;

public sealed record GetResumeByIdQuery(Guid ResumeId, Guid UserId) : IRequest<ResumeInternalResponse>;
