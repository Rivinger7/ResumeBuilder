using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using ResumeBuilder.Api.Models.Paginations;
using ResumeBuilder.Api.Models.Resumes;
using ResumeBuilder.Application.Features.Resumes.Creates;
using ResumeBuilder.Application.Features.Resumes.Deletes;
using ResumeBuilder.Application.Features.Resumes.Gets.Paged;
using ResumeBuilder.Application.Features.Resumes.Gets.Single;
using ResumeBuilder.Application.Features.Resumes.Pdfs.Exports;
using ResumeBuilder.Application.Features.ResumeSections.Creates;
using ResumeBuilder.Application.Features.ResumeSections.Deletes;
using ResumeBuilder.Application.Features.Thumbnails.Creates;
using ResumeBuilder.Application.Features.Thumbnails.Gets;
using ResumeBuilder.Application.Models;
using ResumeBuilder.Domain.Exceptions;
using ResumeBuilder.Domain.Models.Resumes;
using System.IdentityModel.Tokens.Jwt;

namespace ResumeBuilder.Api.Controllers;

[Route("api/resumes")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ResumeController(ISender sender) : ControllerBase
{
    [HttpGet("me")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> GetAllResumesAsync([FromQuery] PaginationRequest paginationRequest)
    {
        string? userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? throw new UnauthorizedException("Invalid or missing user identity");

        GetPagedResumeQuery resumeQuery = new(Guid.Parse(userId), paginationRequest.PageNumber, paginationRequest.PageSize);
        PagedResponse<GetPagedResumeResponse> resumeResponse = await sender.Send(resumeQuery);

        return Ok(resumeResponse);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> GetResumeByIdAsync(Guid id)
    {
        string? userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? throw new UnauthorizedException("Invalid or missing user identity");

        GetResumeByIdQuery getResumeByIdQuery = new(id, Guid.Parse(userId));
        ResumeInternalResponse resumeResponse = await sender.Send(getResumeByIdQuery);

        return Ok(resumeResponse);
    }

    [HttpPost()]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> CreateResumeAsync(CreateResumeRequest createResumeRequest)
    {
        string? userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? throw new UnauthorizedException("Invalid or missing user identity");

        CreateResumeCommand createResumeCommand = new(Guid.Parse(userId), createResumeRequest.Title, createResumeRequest.Description);
        CreateResumeResponse createResumeResponse = await sender.Send(createResumeCommand);

        return Ok(createResumeResponse);
    }

    [HttpPost("sections")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> CreateResumeSectionAsync(CreateResumeSectionCommand createResumeSectionCommand)
    {
        CreateResumeSectionResponse createResumeSectionResponse = await sender.Send(createResumeSectionCommand);

        return Ok(createResumeSectionResponse);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> DeleteResumeAsync(Guid id)
    {
        DeleteResumeCommand deleteResumeCommand = new(id);
        await sender.Send(deleteResumeCommand);

        return Ok();
    }

    [HttpDelete("sections/{id:guid}")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> DeleteResumeSectionAsync(Guid id)
    {
        DeleteResumeSectionCommand deleteResumeSectionCommand = new(id);
        await sender.Send(deleteResumeSectionCommand);

        return Ok();
    }

    // Endpoint riêng — FE tự gọi sau khi save resume xong ở trang detail/edit.
    // ResponseMiddleware.cs của bạn sẽ tự wrap kết quả này vào ApiSuccessResponse<T>.
    [HttpPost("{id:guid}/generate-thumbnail")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> GenerateThumbnail(Guid id, CancellationToken cancellationToken)
    {
        string? userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? throw new UnauthorizedException("Invalid or missing user identity");

        GeneratorResumeThumbnailCommand generatorResumeThumbnailCommand = new(id, Guid.Parse(userId));
        GeneratorResumeThumbnailResponse generatorResumeThumbnailResponse = await sender.Send(generatorResumeThumbnailCommand, cancellationToken);

        return Ok(generatorResumeThumbnailResponse);
    }

    // Ảnh thumbnail public theo id (không auth) — <img src> của trình duyệt không gửi kèm
    // Authorization header, nên endpoint này không thể yêu cầu JWT như các action khác.
    // Id resume là GUID không đoán được nên chấp nhận được về bảo mật, giống hệt model
    // static-file trước đây (cũng không có auth).
    //
    // Cache-Control: no-cache — URL này CỐ ĐỊNH theo id (không có version/query param),
    // nhưng nội dung file trên đĩa thay đổi mỗi lần GenerateThumbnail chạy (Save, rời trang,
    // auto-save). Nếu không có header này, browser mặc định cache ảnh theo URL và sẽ tiếp
    // tục hiển thị bản cũ dù file đã bị ghi đè — đúng bug đã gặp: thumbnail ở trang My Resumes
    // không cập nhật sau khi sửa resume. no-cache (không phải no-store) vẫn cho phép browser
    // giữ bản cache nhưng BẮT BUỘC revalidate với server trước khi dùng — nhẹ hơn no-store
    // (không tải lại ảnh nếu server xác nhận chưa đổi) trong khi vẫn đảm bảo không stale.
    [HttpGet("{id:guid}/thumbnail")]
    [AllowAnonymous]
    public async Task<IActionResult> GetResumeThumbnailAsync(Guid id, CancellationToken cancellationToken)
    {
        GetResumeThumbnailQuery getResumeThumbnailQuery = new(id);
        byte[]? thumbnail = await sender.Send(getResumeThumbnailQuery, cancellationToken);

        if (thumbnail is null)
        {
            return NotFound();
        }

        Response.Headers[HeaderNames.CacheControl] = "no-cache";

        return File(thumbnail, "image/png");
    }

    [HttpGet("{id:guid}/export-pdf")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> ExportResumePdfAsync(Guid id, CancellationToken cancellationToken)
    {
        string? userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? throw new UnauthorizedException("Invalid or missing user identity");

        ExportResumePdfQuery exportResumePdfQuery = new(id, Guid.Parse(userId));
        ExportResumePdfResponse exportResumePdfResponse = await sender.Send(exportResumePdfQuery, cancellationToken);

        return File(exportResumePdfResponse.Content, "application/pdf", exportResumePdfResponse.FileName);
    }
}