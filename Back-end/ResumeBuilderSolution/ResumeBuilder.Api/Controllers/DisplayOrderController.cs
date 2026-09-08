using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeBuilder.Api.Models.Reorders.Entries;
using ResumeBuilder.Api.Models.Reorders.ResumeSections;
using ResumeBuilder.Application.Features.Reorders.Entries.Updates;
using ResumeBuilder.Application.Features.Reorders.ResumeSections.Updates;

namespace ResumeBuilder.Api.Controllers;

[Route("api/display-orders")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public sealed class DisplayOrderController(ISender sender) : ControllerBase
{
    [HttpPatch("resume-sections/{resumeSectionId:guid}/entries/{id:guid}")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> UpdateDisplayOrder(Guid resumeSectionId, Guid id, UpdateDisplayOrderEntryRequest request)
    {
        UpdateDisplayOrderEntryCommand updateDisplayOrderEntryCommand = new(resumeSectionId, id, request.Type, request.NewDisplayOrder);
        UpdateDisplayOrderEntryResponse updateDisplayOrderEntryResponse = await sender.Send(updateDisplayOrderEntryCommand);

        return Ok(updateDisplayOrderEntryResponse);
    }

    [HttpPatch("resumes/{resumeId:guid}/resume-sections/{id:guid}")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> UpdateDisplayOrderResumeSection(Guid resumeId, Guid id, UpdateDisplayOrderResumeSectionRequest request)
    {
        UpdateDisplayOrderResumeSectionCommand updateDisplayOrderResumeSectionCommand = new(resumeId, id, request.NewDisplayOrder);
        UpdateDisplayOrderResumeSectionResponse updateDisplayOrderResumeSectionResponse = await sender.Send(updateDisplayOrderResumeSectionCommand);

        return Ok(updateDisplayOrderResumeSectionResponse);
    }
}
