using MediatR;

namespace ResumeBuilder.Application.Features.Reorders.ResumeSections.Updates;

public sealed record UpdateDisplayOrderResumeSectionCommand(Guid ResumeId, Guid Id, int NewDisplayOrder) : IRequest<UpdateDisplayOrderResumeSectionResponse>;