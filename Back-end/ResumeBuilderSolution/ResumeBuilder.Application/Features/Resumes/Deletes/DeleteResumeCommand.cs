using MediatR;

namespace ResumeBuilder.Application.Features.Resumes.Deletes;

public sealed record DeleteResumeCommand(Guid Id) : IRequest<DeleteResumeResponse>;