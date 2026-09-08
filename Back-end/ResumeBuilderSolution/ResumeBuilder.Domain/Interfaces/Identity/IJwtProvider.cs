using ResumeBuilder.Domain.Models.Authentication;

namespace ResumeBuilder.Domain.Interfaces.Identity;

public interface IJwtProvider
{
    string GenerateAccessTokenString(JwtPayloadInternalRequest payload);
    string GenerateRefreshTokenString();
}
