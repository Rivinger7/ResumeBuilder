namespace ResumeBuilder.Application.Features.Authentication.Login;

public sealed record LoginWithGoogleEntryResponse(string AccessToken, string RefreshToken);