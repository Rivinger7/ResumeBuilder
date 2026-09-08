namespace ResumeBuilder.Api.Models.Authentication;

public sealed record RegisterRequest(string Email, string Password, string FullName);
