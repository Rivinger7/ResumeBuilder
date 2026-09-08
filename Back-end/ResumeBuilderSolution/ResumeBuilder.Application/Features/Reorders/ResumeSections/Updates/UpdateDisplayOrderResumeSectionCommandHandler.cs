using MediatR;
using ResumeBuilder.Domain.Entities.Resumes;
using ResumeBuilder.Domain.Interfaces.Services;

namespace ResumeBuilder.Application.Features.Reorders.ResumeSections.Updates;

public sealed class UpdateDisplayOrderResumeSectionCommandHandler(IDisplayOrderService displayOrderService) : IRequestHandler<UpdateDisplayOrderResumeSectionCommand, UpdateDisplayOrderResumeSectionResponse>
{
    private readonly IDisplayOrderService _displayOrderService = displayOrderService;

    public async Task<UpdateDisplayOrderResumeSectionResponse> Handle(UpdateDisplayOrderResumeSectionCommand request, CancellationToken cancellationToken)
    {
        await _displayOrderService.BulkUpdateDisplayOrderSectionsByResumeIdAsync<ResumeSection>(request.ResumeId, request.Id, request.NewDisplayOrder, cancellationToken);

        return new UpdateDisplayOrderResumeSectionResponse();
    }
}