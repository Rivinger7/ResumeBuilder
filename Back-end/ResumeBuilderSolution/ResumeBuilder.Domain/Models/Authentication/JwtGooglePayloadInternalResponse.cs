namespace ResumeBuilder.Domain.Models.Authentication;

public sealed record class JwtGooglePayloadInternalResponse(string Sub, string Email, bool? EmailVerified, string FullName, string? GivenName, string? FamilyName, string? Picture);
