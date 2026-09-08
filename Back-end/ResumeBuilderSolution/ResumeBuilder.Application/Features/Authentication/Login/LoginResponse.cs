namespace ResumeBuilder.Application.Features.Authentication.Login;

public sealed record LoginResponse(string AccessToken, string RefreshToken);