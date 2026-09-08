using MediatR;

namespace ResumeBuilder.Application.Features.ResumeSections.Deletes;

public sealed record DeleteResumeSectionCommand(Guid Id) : IRequest<DeleteResumeSectionResponse>;
