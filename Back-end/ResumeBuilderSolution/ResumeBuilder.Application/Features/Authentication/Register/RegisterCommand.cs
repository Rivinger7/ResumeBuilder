using MediatR;

namespace ResumeBuilder.Application.Features.Authentication.Register;

public sealed record RegisterCommand(string Email, string Password, string FullName, string? IpAddress, string? UserAgent) : IRequest<RegisterResponse>;
