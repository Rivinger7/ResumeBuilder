namespace ResumeBuilder.Domain.Models.Authentication;

public sealed record JwtPayloadWithPasswordHashInternalRequest(Guid UserId, string Email, string FullName, string PasswordHash);
