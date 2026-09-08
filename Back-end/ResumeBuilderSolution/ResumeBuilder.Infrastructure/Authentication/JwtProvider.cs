using Microsoft.IdentityModel.Tokens;
using ResumeBuilder.Domain.Helpers;
using ResumeBuilder.Domain.Interfaces.Identity;
using ResumeBuilder.Domain.Models.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ResumeBuilder.Infrastructure.Authentication;

internal sealed class JwtProvider(JwtOptions options) : IJwtProvider
{
    public string GenerateAccessTokenString(JwtPayloadInternalRequest payload)
    {
        Claim[] claims =
        [
            new(JwtRegisteredClaimNames.Sub, payload.UserId.ToString()),
            new(JwtRegisteredClaimNames.Email, payload.Email),
            new(JwtRegisteredClaimNames.Name, payload.FullName),
            new(ClaimTypes.Role, payload.Role.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        ];

        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(options.SecretKey));

        SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new(
            issuer: options.Issuer,
            audience: options.Audience,
            claims: claims,
            expires: CustomTimeProvider.AddMinutesToUtcNow(options.ExpirationInMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshTokenString()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
}
