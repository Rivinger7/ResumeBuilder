using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeBuilder.Application.Features.Profiles;
using ResumeBuilder.Domain.Exceptions;
using System.IdentityModel.Tokens.Jwt;

namespace ResumeBuilder.Api.Controllers;

[Route("api/users")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UserController(ISender sender) : ControllerBase
{
    [HttpGet("profile/me")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> GetProfileAsync()
    {
        string? userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? throw new UnauthorizedException("Invalid or missing user identity");

        GetProfileQuery profileCommand = new(Guid.Parse(userId));
        GetProfileResponse profileResponse = await sender.Send(profileCommand);

        return Ok(profileResponse);
    }
}
