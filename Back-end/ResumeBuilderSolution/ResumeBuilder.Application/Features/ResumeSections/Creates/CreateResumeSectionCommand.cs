using MediatR;
using ResumeBuilder.Domain.Enums;

namespace ResumeBuilder.Application.Features.ResumeSections.Creates;

public sealed record CreateResumeSectionCommand(Guid ResumeId, ResumeSectionType Type, int DisplayOrder) : IRequest<CreateResumeSectionResponse>;