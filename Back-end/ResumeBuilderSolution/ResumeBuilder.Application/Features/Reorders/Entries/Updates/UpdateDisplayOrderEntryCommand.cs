using MediatR;
using ResumeBuilder.Domain.Enums;

namespace ResumeBuilder.Application.Features.Reorders.Entries.Updates;

public sealed record UpdateDisplayOrderEntryCommand(Guid ResumeSectionId, Guid Id, ResumeSectionType Type, int NewDisplayOrder) : IRequest<UpdateDisplayOrderEntryResponse>;