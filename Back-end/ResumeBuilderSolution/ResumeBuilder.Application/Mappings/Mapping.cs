using Mapster;
using ResumeBuilder.Domain.Entities.Identity;
using ResumeBuilder.Domain.Models.Authentication;

namespace ResumeBuilder.Application.Mappings;

public sealed class Mapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, JwtPayloadWithPasswordHashInternalRequest>()
            .Map(dest => dest.UserId, src => src.Id);

        config.NewConfig<User, JwtPayloadInternalRequest>()
            .Map(dest => dest.UserId, src => src.Id);
    }
}
