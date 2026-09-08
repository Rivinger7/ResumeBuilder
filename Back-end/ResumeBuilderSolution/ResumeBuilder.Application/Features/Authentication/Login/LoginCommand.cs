using MediatR;

namespace ResumeBuilder.Application.Features.Authentication.Login;

public sealed record LoginCommand(string Email, string Password, string? IpAddress, string? UserAgent) : IRequest<LoginResponse>;