namespace ResumeBuilder.Application.Features.Profiles;

public sealed record GetProfileResponse(string Email, string FullName, string AvatarUrl);