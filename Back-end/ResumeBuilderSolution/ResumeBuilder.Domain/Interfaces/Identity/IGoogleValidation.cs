using ResumeBuilder.Domain.Models.Authentication;

namespace ResumeBuilder.Domain.Interfaces.Identity;

public interface IGoogleValidation
{
    Task<JwtGooglePayloadInternalResponse> ValidateGoogleTokenAsync(string idToken);
}
