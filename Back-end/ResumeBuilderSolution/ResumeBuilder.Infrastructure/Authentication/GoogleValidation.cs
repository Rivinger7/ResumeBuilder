using Google.Apis.Auth;
using ResumeBuilder.Domain.Exceptions;
using ResumeBuilder.Domain.Interfaces.Identity;
using ResumeBuilder.Domain.Models.Authentication;

namespace ResumeBuilder.Infrastructure.Authentication;

internal sealed class GoogleValidation(JwtOptions jwtOptions) : IGoogleValidation
{
    private readonly JwtOptions _jwtOptions = jwtOptions;

    public async Task<JwtGooglePayloadInternalResponse> ValidateGoogleTokenAsync(string idToken)
    {
        GoogleJsonWebSignature.Payload payload;

        try
        {
            payload = await GoogleJsonWebSignature.ValidateAsync(idToken, new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = [_jwtOptions.GoogleClientId]
            });
        }
        catch (InvalidJwtException)
        {
            throw new UnauthorizedException("Invalid Google token");
        }

        return new JwtGooglePayloadInternalResponse(
                Sub: payload.Subject,
                Email: payload.Email,
                EmailVerified: payload.EmailVerified,
                FullName: payload.Name,
                GivenName: payload.GivenName,
                FamilyName: payload.FamilyName,
                Picture: payload.Picture
            );
    }
}
