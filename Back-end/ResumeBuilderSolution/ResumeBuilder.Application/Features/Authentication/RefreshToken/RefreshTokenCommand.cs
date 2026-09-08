using MediatR;

namespace ResumeBuilder.Application.Features.Authentication.RefreshToken;

public sealed record RefreshTokenCommand(string OldRefreshToken, string? IpAddress, string? UserAgent) : IRequest<RefreshTokenResponse>;