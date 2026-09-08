using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using ResumeBuilder.Api.Models.Entries;
using ResumeBuilder.Application.Features.Entries.Certificates.Creates;
using ResumeBuilder.Application.Features.Entries.Certificates.Deletes;
using ResumeBuilder.Application.Features.Entries.Certificates.Updates;
using ResumeBuilder.Application.Features.Entries.Educations.Creates;
using ResumeBuilder.Application.Features.Entries.Educations.Deletes;
using ResumeBuilder.Application.Features.Entries.Educations.Updates;
using ResumeBuilder.Application.Features.Entries.Experiences.Creates;
using ResumeBuilder.Application.Features.Entries.Experiences.Deletes;
using ResumeBuilder.Application.Features.Entries.Experiences.Updates;
using ResumeBuilder.Application.Features.Entries.Languages.Creates;
using ResumeBuilder.Application.Features.Entries.Languages.Deletes;
using ResumeBuilder.Application.Features.Entries.Languages.Updates;
using ResumeBuilder.Application.Features.Entries.Objectives.Creates;
using ResumeBuilder.Application.Features.Entries.Objectives.Deletes;
using ResumeBuilder.Application.Features.Entries.Objectives.Updates;
using ResumeBuilder.Application.Features.Entries.PersonalInformations.Photos.Gets;
using ResumeBuilder.Application.Features.Entries.PersonalInformations.Photos.Uploads;
using ResumeBuilder.Application.Features.Entries.PersonalInformations.Updates;
using ResumeBuilder.Application.Features.Entries.Projects.Creates;
using ResumeBuilder.Application.Features.Entries.Projects.Deletes;
using ResumeBuilder.Application.Features.Entries.Projects.Updates;
using ResumeBuilder.Application.Features.Entries.Skills.Creates;
using ResumeBuilder.Application.Features.Entries.Skills.Deletes;
using ResumeBuilder.Application.Features.Entries.Skills.Updates;
using ResumeBuilder.Application.Features.Entries.Summaries.Creates;
using ResumeBuilder.Application.Features.Entries.Summaries.Deletes;
using ResumeBuilder.Application.Features.Entries.Summaries.Updates;
using ResumeBuilder.Domain.Exceptions;

namespace ResumeBuilder.Api.Controllers;

[Route("api/entries")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public sealed class EntryController(ISender sender) : ControllerBase
{
    #region Certificates
    [HttpPost("certificates")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> CreateCertificateEntryAsync(CreateCertificateEntryCommand createCertificateEntryCommand)
    {
        CreateCertificateEntryResponse createCertificateEntryResponse = await sender.Send(createCertificateEntryCommand);
        return Ok(createCertificateEntryResponse);
    }

    [HttpPatch("certificates/{id:guid}")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> UpdateCertificateEntryAsync(Guid id, UpdateCertificateEntryRequest request)
    {
        UpdateCertificateEntryCommand updateCertificateEntryCommand = new(id, request.Title, request.CertificateUrl, request.Description);
        UpdateCertificateEntryResponse updateCertificateEntryResponse = await sender.Send(updateCertificateEntryCommand);

        return Ok(updateCertificateEntryResponse);
    }

    [HttpPatch("certificates/{id:guid}/style")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> UpdateCertificateEntryStyleAsync(Guid id, UpdateCertificateEntryStyleRequest request)
    {
        UpdateCertificateEntryStyleCommand updateCertificateEntryStyleCommand = new(id, request.CertificateLayout, request.CertificateGridColumn, request.CertificateRowSpacing, request.IsStartRowsWithBullet, request.SubinfoStyle);
        UpdateCertificateEntryStyleResponse updateCertificateEntryStyleResponse = await sender.Send(updateCertificateEntryStyleCommand);

        return Ok(updateCertificateEntryStyleResponse);
    }

    [HttpDelete("certificates/{id:guid}")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> DeleteCertificateEntryAsync(Guid id)
    {
        DeleteCertificateEntryCommand deleteCertificateEntryCommand = new(id);
        DeleteCertificateEntryResponse deleteCertificateEntryResponse = await sender.Send(deleteCertificateEntryCommand);

        return Ok(deleteCertificateEntryResponse);
    }
    #endregion

    #region Educations
    [HttpPost("educations")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> CreateEducationEntryAsync(CreateEducationEntryCommand createEducationEntryCommand)
    {
        CreateEducationEntryResponse createEducationEntryResponse = await sender.Send(createEducationEntryCommand);
        return Ok(createEducationEntryResponse);
    }

    [HttpPatch("educations/{id:guid}")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> UpdateEducationEntryAsync(Guid id, UpdateEducationEntryRequest request)
    {
        UpdateEducationEntryCommand updateEducationEntryCommand = new(id, request.SchoolName, request.Degree, request.Major, request.GPA, request.IsCurrent, request.StartDate, request.EndDate, request.Location, request.Description, request.IsByOrder);
        UpdateEducationEntryResponse updateEducationEntryResponse = await sender.Send(updateEducationEntryCommand);

        return Ok(updateEducationEntryResponse);
    }

    [HttpDelete("educations/{id:guid}")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> DeleteEducationEntryAsync(Guid id)
    {
        DeleteEducationEntryCommand deleteEducationEntryCommand = new(id);
        DeleteEducationEntryResponse deleteEducationEntryResponse = await sender.Send(deleteEducationEntryCommand);

        return Ok(deleteEducationEntryResponse);
    }
    #endregion

    #region Experiences
    [HttpPost("experiences")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> CreateExperienceEntryAsync(CreateExperienceEntryCommand createExperienceEntryCommand)
    {
        CreateExperienceEntryResponse createExperienceEntryResponse = await sender.Send(createExperienceEntryCommand);
        return Ok(createExperienceEntryResponse);
    }

    [HttpPatch("experiences/{id:guid}")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> UpdateExperienceEntryAsync(Guid id, UpdateExperienceEntryRequestt request)
    {
        UpdateExperienceEntryCommand updateExperienceEntryCommand = new(id, request.CompanyName, request.Position, request.StartDate, request.EndDate, request.IsCurrent, request.Location, request.Description, request.IsByOrder);
        UpdateExperienceEntryResponse updateExperienceEntryResponse = await sender.Send(updateExperienceEntryCommand);

        return Ok(updateExperienceEntryResponse);
    }

    [HttpDelete("experiences/{id:guid}")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> DeleteExperienceEntryAsync(Guid id)
    {
        DeleteExperienceEntryCommand deleteExperienceEntryCommand = new(id);
        DeleteExperienceEntryResponse deleteExperienceEntryResponse = await sender.Send(deleteExperienceEntryCommand);

        return Ok(deleteExperienceEntryResponse);
    }
    #endregion

    #region Languages
    [HttpPost("languages")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> CreateLanguageEntryAsync(CreateLanguageEntryCommand createLanguageEntryCommand)
    {
        CreateLanguageEntryResponse createLanguageEntryResponse = await sender.Send(createLanguageEntryCommand);
        return Ok(createLanguageEntryResponse);
    }

    [HttpPatch("languages/{id:guid}")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> UpdateLanguageEntryAsync(Guid id, UpdateLanguageEntryRequest request)
    {
        UpdateLanguageEntryCommand updateLanguageEntryCommand = new(id, request.LanguageName, request.Proficiency);
        UpdateLanguageEntryResponse updateLanguageEntryResponse = await sender.Send(updateLanguageEntryCommand);

        return Ok(updateLanguageEntryResponse);
    }

    [HttpPatch("languages/{id:guid}/style")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> UpdateLanguageEntryStyleAsync(Guid id, UpdateLanguageEntryStyleRequest request)
    {
        UpdateLanguageEntryStyleCommand updateLanguageEntryStyleCommand = new(id, request.LanguageLayout, request.LanguageGridColumn, request.LanguageRowSpacing, request.IsStartRowsWithBullet, request.SubinfoStyle);
        UpdateLanguageEntryStyleResponse updateLanguageEntryStyleResponse = await sender.Send(updateLanguageEntryStyleCommand);
        return Ok(updateLanguageEntryStyleResponse);
    }

    [HttpDelete("languages/{id:guid}")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> DeleteLanguageEntryAsync(Guid id)
    {
        DeleteLanguageEntryCommand deleteLanguageEntryCommand = new(id);
        DeleteLanguageEntryResponse deleteLanguageEntryResponse = await sender.Send(deleteLanguageEntryCommand);

        return Ok(deleteLanguageEntryResponse);
    }
    #endregion

    #region Skills
    [HttpPost("skills")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> CreateSkillEntryAsync(CreateSkillEntryCommand createSkillEntryCommand)
    {
        CreateSkillEntryResponse createSkillEntryResponse = await sender.Send(createSkillEntryCommand);
        return Ok(createSkillEntryResponse);
    }

    [HttpPatch("skills/{id:guid}")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> UpdateSkillEntryAsync(Guid id, UpdateSkillEntryRequest request)
    {
        UpdateSkillEntryCommand updateSkillEntryCommand = new(id, request.SkillName, request.Description, request.SkillLevel);
        UpdateSkillEntryResponse updateSkillEntryResponse = await sender.Send(updateSkillEntryCommand);

        return Ok(updateSkillEntryResponse);
    }

    [HttpPatch("skills/{id:guid}/style")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> UpdateSkillEntryStyleAsync(Guid id, UpdateSkillEntryStyleRequest request)
    {
        UpdateSkillEntryStyleCommand updateSkillEntryStyleCommand = new(id, request.SkillLayout, request.SkillGridColumn, request.SkillRowSpacing, request.IsStartRowsWithBullet, request.SubinfoStyle);
        UpdateSkillEntryStyleResponse updateSkillEntryStyleResponse = await sender.Send(updateSkillEntryStyleCommand);

        return Ok(updateSkillEntryStyleResponse);
    }

    [HttpDelete("skills/{id:guid}")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> DeleteSkillEntryAsync(Guid id)
    {
        DeleteSkillEntryCommand deleteSkillEntryCommand = new(id);
        DeleteSkillEntryResponse deleteSkillEntryResponse = await sender.Send(deleteSkillEntryCommand);

        return Ok(deleteSkillEntryResponse);
    }
    #endregion

    #region Objectives
    [HttpPost("objectives")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> CreateObjectiveEntryAsync(CreateObjectiveEntryCommand createObjectiveEntryCommand)
    {
        CreateObjectiveEntryResponse createObjectiveEntryResponse = await sender.Send(createObjectiveEntryCommand);
        return Ok(createObjectiveEntryResponse);
    }

    [HttpPatch("objectives/{id:guid}")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> UpdateObjectiveEntryAsync(Guid id, UpdateObjectiveEntryRequest request)
    {
        UpdateObjectiveEntryCommand updateObjectiveEntryCommand = new(id, request.Title, request.SubTitle, request.Description);
        UpdateObjectiveEntryResponse updateObjectiveEntryResponse = await sender.Send(updateObjectiveEntryCommand);

        return Ok(updateObjectiveEntryResponse);
    }

    [HttpDelete("objectives/{id:guid}")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> DeleteObjectiveEntryAsync(Guid id)
    {
        DeleteObjectiveEntryCommand deleteObjectiveEntryCommand = new(id);
        DeleteObjectiveEntryResponse deleteObjectiveEntryResponse = await sender.Send(deleteObjectiveEntryCommand);

        return Ok(deleteObjectiveEntryResponse);
    }
    #endregion

    #region PersonalInformations
    [HttpPatch("personal-information/{id:guid}")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> UpdatePersonalInformationEntryAsync(Guid id, UpdatePersonalInformationEntryRequest request)
    {
        UpdatePersonalInformationEntryCommand updatePersonalInformationEntryCommand = new(id, request.FullName, request.ProfessionalTitle, request.Email, request.PhoneNumber, request.Address, request.Website, request.LinkedIn, request.GitHub, request.PhotoUrl);
        UpdatePersonalInformationEntryResponse updatePersonalInformationEntryResponse = await sender.Send(updatePersonalInformationEntryCommand);

        return Ok(updatePersonalInformationEntryResponse);
    }

    [HttpPatch("personal-information/{id:guid}/style")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> UpdatePersonalInformationEntryStyleAsync(Guid id, UpdatePersonalInformationEntryStyleRequest request)
    {
        UpdatePersonalInformationEntryStyleCommand updatePersonalInformationEntryStyleCommand = new(id, request.PersonalInformationTextAlignment, request.PersonalInformationLayout, request.PersonalInformationMarkerStyle, request.PersonalInformationIconStyle, request.IsTitleProfessionalTitleOnSameLine);
        UpdatePersonalInformationEntryStyleResponse updatePersonalInformationEntryStyleResponse = await sender.Send(updatePersonalInformationEntryStyleCommand);

        return Ok(updatePersonalInformationEntryStyleResponse);
    }

    [HttpPost("personal-information/{id:guid}/photo")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> UploadPersonalInformationPhotoAsync(Guid id, IFormFile file, CancellationToken cancellationToken)
    {
        if (file is null || file.Length == 0)
        {
            throw new BadRequestException("No file uploaded.");
        }

        string fileExtension = Path.GetExtension(file.FileName);

        await using Stream stream = file.OpenReadStream();
        UploadPersonalInformationPhotoCommand uploadPersonalInformationPhotoCommand = new(id, stream, fileExtension);
        UploadPersonalInformationPhotoResponse uploadPersonalInformationPhotoResponse = await sender.Send(uploadPersonalInformationPhotoCommand, cancellationToken);

        return Ok(uploadPersonalInformationPhotoResponse);
    }

    // Public, unauthenticated — <img src> is a native browser request that can't attach
    // an Authorization header, same reasoning as the resume thumbnail endpoint.
    [HttpGet("personal-information/{id:guid}/photo")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPersonalInformationPhotoAsync(Guid id, CancellationToken cancellationToken)
    {
        GetPersonalInformationPhotoQuery getPersonalInformationPhotoQuery = new(id);
        (byte[] Content, string ContentType)? photo = await sender.Send(getPersonalInformationPhotoQuery, cancellationToken);

        if (photo is null)
        {
            return NotFound();
        }

        Response.Headers[HeaderNames.CacheControl] = "no-cache";

        return File(photo.Value.Content, photo.Value.ContentType);
    }
    #endregion

    #region Projects
    [HttpPost("projects")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> CreateProjectEntryAsync(CreateProjectEntryCommand createProjectEntryCommand)
    {
        CreateProjectEntryResponse createProjectEntryResponse = await sender.Send(createProjectEntryCommand);
        return Ok(createProjectEntryResponse);
    }

    [HttpPatch("projects/{id:guid}")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> UpdateProjectEntryAsync(Guid id, UpdateProjectEntryRequest request)
    {
        UpdateProjectEntryCommand updateProjectEntryCommand = new(id, request.Title, request.SubTitle, request.StartDate, request.EndDate, request.ProjectUrl, request.RepositoryUrl, request.Description);
        UpdateProjectEntryResponse updateProjectEntryResponse = await sender.Send(updateProjectEntryCommand);

        return Ok(updateProjectEntryResponse);
    }

    [HttpDelete("projects/{id:guid}")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> DeleteProjectEntryAsync(Guid id)
    {
        DeleteProjectEntryCommand deleteProjectEntryCommand = new(id);
        DeleteProjectEntryResponse deleteProjectEntryResponse = await sender.Send(deleteProjectEntryCommand);

        return Ok(deleteProjectEntryResponse);
    }
    #endregion

    #region Summaries
    [HttpPost("summaries")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> CreateSummaryEntryAsync(CreateSummaryEntryCommand createSummaryEntryCommand)
    {
        CreateSummaryEntryResponse createSummaryEntryResponse = await sender.Send(createSummaryEntryCommand);
        return Ok(createSummaryEntryResponse);
    }

    [HttpPut("summaries/{id:guid}")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> UpdateSummaryEntryAsync(Guid id, UpdateSummaryEntryRequest request)
    {
        UpdateSummaryEntryCommand updateSummaryEntryCommand = new(id, request.Summary);
        UpdateSummaryEntryResponse updateSummaryEntryResponse = await sender.Send(updateSummaryEntryCommand);

        return Ok(updateSummaryEntryResponse);
    }

    [HttpDelete("summaries/{id:guid}")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> DeleteSummaryEntryAsync(Guid id)
    {
        DeleteSummaryEntryCommand deleteSummaryEntryCommand = new(id);
        DeleteSummaryEntryResponse deleteSummaryEntryResponse = await sender.Send(deleteSummaryEntryCommand);

        return Ok(deleteSummaryEntryResponse);
    }
    #endregion
}

