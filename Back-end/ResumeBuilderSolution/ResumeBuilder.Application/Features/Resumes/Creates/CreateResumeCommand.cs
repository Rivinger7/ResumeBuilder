using MediatR;

namespace ResumeBuilder.Application.Features.Resumes.Creates;

public sealed record CreateResumeCommand(Guid UserId, string Title, string? Description) : IRequest<CreateResumeResponse>;