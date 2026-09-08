using MediatR;
using ResumeBuilder.Domain.Entities.Resumes.Entries;
using ResumeBuilder.Domain.Enums;
using ResumeBuilder.Domain.Exceptions;
using ResumeBuilder.Domain.Interfaces.Services;

namespace ResumeBuilder.Application.Features.Reorders.Entries.Updates;

public sealed class UpdateDisplayOrderEntryCommandHandler(IDisplayOrderService displayOrderService) : IRequestHandler<UpdateDisplayOrderEntryCommand, UpdateDisplayOrderEntryResponse>
{
    private readonly IDisplayOrderService _displayOrderService = displayOrderService;

    public async Task<UpdateDisplayOrderEntryResponse> Handle(UpdateDisplayOrderEntryCommand request, CancellationToken cancellationToken)
    {
        Task ReorderTask() => request.Type switch
        {
            ResumeSectionType.Certificates => _displayOrderService.BulkUpdateDisplayOrderEntriesByResumeSectionIdAsync<CertificateEntry>(request.ResumeSectionId, request.Id, request.NewDisplayOrder, cancellationToken),
            ResumeSectionType.Education => _displayOrderService.BulkUpdateDisplayOrderEntriesByResumeSectionIdAsync<EducationEntry>(request.ResumeSectionId, request.Id, request.NewDisplayOrder, cancellationToken),
            ResumeSectionType.Experience => _displayOrderService.BulkUpdateDisplayOrderEntriesByResumeSectionIdAsync<ExperienceEntry>(request.ResumeSectionId, request.Id, request.NewDisplayOrder, cancellationToken),
            ResumeSectionType.Languages => _displayOrderService.BulkUpdateDisplayOrderEntriesByResumeSectionIdAsync<LanguageEntry>(request.ResumeSectionId, request.Id, request.NewDisplayOrder, cancellationToken),
            ResumeSectionType.Objective => _displayOrderService.BulkUpdateDisplayOrderEntriesByResumeSectionIdAsync<ObjectiveEntry>(request.ResumeSectionId, request.Id, request.NewDisplayOrder, cancellationToken),
            ResumeSectionType.Projects => _displayOrderService.BulkUpdateDisplayOrderEntriesByResumeSectionIdAsync<ProjectEntry>(request.ResumeSectionId, request.Id, request.NewDisplayOrder, cancellationToken),
            ResumeSectionType.Summary => _displayOrderService.BulkUpdateDisplayOrderEntriesByResumeSectionIdAsync<SummaryEntry>(request.ResumeSectionId, request.Id, request.NewDisplayOrder, cancellationToken),
            ResumeSectionType.Skills => _displayOrderService.BulkUpdateDisplayOrderEntriesByResumeSectionIdAsync<SkillEntry>(request.ResumeSectionId, request.Id, request.NewDisplayOrder, cancellationToken),
            _ => throw new BadRequestException($"Reordering entries for the section type '{request.Type}' is not supported.")
        };

        await ReorderTask();

        return new UpdateDisplayOrderEntryResponse();
    }
}