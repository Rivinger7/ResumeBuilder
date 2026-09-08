namespace ResumeBuilder.Domain.Models.Authentication;

public sealed record RefreshTokenInternalRequest(Guid UserId, string IpAddress, string UserAgent);
