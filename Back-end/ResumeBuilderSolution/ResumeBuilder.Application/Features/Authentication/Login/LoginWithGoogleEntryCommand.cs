using MediatR;

namespace ResumeBuilder.Application.Features.Authentication.Login;

public sealed record LoginWithGoogleEntryCommand(string IdToken, string? IpAddress, string? UserAgent) : IRequest<LoginWithGoogleEntryResponse>;