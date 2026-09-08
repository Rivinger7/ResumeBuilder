using MediatR;

namespace ResumeBuilder.Application.Features.Authentication.Logout;

public sealed record LogoutCommand(string RefreshToken, string? IpAddress) : IRequest<LogoutResponse>;