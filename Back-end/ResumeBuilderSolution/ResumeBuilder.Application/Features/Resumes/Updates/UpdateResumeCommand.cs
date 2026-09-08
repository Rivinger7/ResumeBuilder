using MediatR;

namespace ResumeBuilder.Application.Features.Resumes.Updates;

public sealed record UpdateResumeCommand() : IRequest<UpdateResumeResponse>;