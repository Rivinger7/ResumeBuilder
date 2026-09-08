using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeBuilder.Api.Models.Authentication;
using ResumeBuilder.Application.Features.Authentication.Login;
using ResumeBuilder.Application.Features.Authentication.Logout;
using ResumeBuilder.Application.Features.Authentication.RefreshToken;
using ResumeBuilder.Application.Features.Authentication.Register;
using ResumeBuilder.Domain.Exceptions;

namespace ResumeBuilder.Api.Controllers;

[Route("api/auth")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public sealed class AuthenticationController(ISender sender) : ControllerBase
{
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterAsync(RegisterRequest registerRequest)
    {
        string? ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        string? userAgent = HttpContext.Request.Headers.UserAgent.ToString();

        RegisterCommand registerCommand = new(registerRequest.Email, registerRequest.Password, registerRequest.FullName, ipAddress, userAgent);
        RegisterResponse registerResponse = await sender.Send(registerCommand);

        HttpContext.Response.Cookies.Append(
            "refreshToken",
            registerResponse.RefreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(30)
            });

        return Ok(registerResponse.AccessToken);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginAsync(LoginRequest loginRequest)
    {
        string? ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        string? userAgent = HttpContext.Request.Headers.UserAgent.ToString();

        LoginCommand loginCommand = new(loginRequest.Email, loginRequest.Password, ipAddress, userAgent);
        LoginResponse loginResponse = await sender.Send(loginCommand);

        HttpContext.Response.Cookies.Append(
            "refreshToken",
            loginResponse.RefreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(30)
            });

        return Ok(loginResponse.AccessToken);
    }

    [HttpPost("login-with-google")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginWithGoogleAsync(LoginWithGoogleRequest loginWithGoogleRequest)
    {
        string? ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        string? userAgent = HttpContext.Request.Headers.UserAgent.ToString();

        LoginWithGoogleEntryCommand loginWithGoogleEntryCommand = new(loginWithGoogleRequest.IdToken, ipAddress, userAgent);
        LoginWithGoogleEntryResponse loginWithGoogleEntryResponse = await sender.Send(loginWithGoogleEntryCommand);

        HttpContext.Response.Cookies.Append(
            "refreshToken",
            loginWithGoogleEntryResponse.RefreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(30)
            });

        return Ok(loginWithGoogleEntryResponse.AccessToken);
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh()
    {
        string refreshToken = Request.Cookies["refreshToken"] ?? throw new UnauthorizedException("Refresh token not found");

        string? ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        string? userAgent = HttpContext.Request.Headers.UserAgent.ToString();

        RefreshTokenCommand refreshTokenCommand = new(refreshToken, ipAddress, userAgent);
        RefreshTokenResponse refreshTokenResponse = await sender.Send(refreshTokenCommand);

        HttpContext.Response.Cookies.Append(
            "refreshToken",
            refreshTokenResponse.RefreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(30)
            });

        return Ok(refreshTokenResponse.AccessToken);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        string? refreshToken = Request.Cookies["refreshToken"];

        if (!string.IsNullOrEmpty(refreshToken))
        {
            string? ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            LogoutCommand logoutCommand = new(refreshToken, ipAddress);
            await sender.Send(logoutCommand);
        }

        Response.Cookies.Delete("refreshToken");

        return Ok();
    }
}
