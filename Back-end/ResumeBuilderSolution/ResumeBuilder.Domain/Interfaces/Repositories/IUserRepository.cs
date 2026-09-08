using ResumeBuilder.Domain.Entities.Identity;
using ResumeBuilder.Domain.Interfaces.Persistence;
using ResumeBuilder.Domain.Models.Authentication;
using ResumeBuilder.Domain.Models.Profiles;

namespace ResumeBuilder.Domain.Interfaces.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<JwtPayloadWithPasswordHashInternalRequest?> GetJwtPayloadWithPasswordHashByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<JwtPayloadInternalRequest?> GetJwtPayloadByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ProfileInternalResponse?> GetProfileByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
