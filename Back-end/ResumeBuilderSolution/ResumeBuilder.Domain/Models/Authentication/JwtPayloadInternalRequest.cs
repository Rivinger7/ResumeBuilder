using ResumeBuilder.Domain.Enums;

namespace ResumeBuilder.Domain.Models.Authentication;

public sealed record JwtPayloadInternalRequest(Guid UserId, UserRole Role, string Email, string FullName);
