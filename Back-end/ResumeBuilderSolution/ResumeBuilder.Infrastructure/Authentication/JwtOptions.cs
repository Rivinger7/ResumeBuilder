namespace ResumeBuilder.Infrastructure.Authentication;

public sealed class JwtOptions
{
    public string Issuer { get; init; } = null!;

    public string Audience { get; init; } = null!;

    public string SecretKey { get; init; } = null!;

    public int ExpirationInMinutes { get; init; }

    public string GoogleClientId { get; init; } = null!;
    public string GoogleClientSecret { get; init; } = null!;
}