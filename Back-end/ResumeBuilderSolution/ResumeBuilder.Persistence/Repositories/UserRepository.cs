using Mapster;
using Microsoft.EntityFrameworkCore;
using ResumeBuilder.Domain.Entities.Identity;
using ResumeBuilder.Domain.Interfaces.Repositories;
using ResumeBuilder.Domain.Models.Authentication;
using ResumeBuilder.Domain.Models.Profiles;
using ResumeBuilder.Persistence.Context;

namespace ResumeBuilder.Persistence.Repositories;

internal class UserRepository(ApplicationDbContext context) : Repository<User>(context), IUserRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AnyAsync(x => x.Email == email, cancellationToken);
    }

    public async Task<JwtPayloadWithPasswordHashInternalRequest?> GetJwtPayloadWithPasswordHashByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Where(x => x.Email == email)
            .ProjectToType<JwtPayloadWithPasswordHashInternalRequest>()
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<JwtPayloadInternalRequest?> GetJwtPayloadByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Where(x => x.Id == id)
            .ProjectToType<JwtPayloadInternalRequest>()
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ProfileInternalResponse?> GetProfileByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Where(x => x.Id == id)
            .ProjectToType<ProfileInternalResponse>()
            .FirstOrDefaultAsync(cancellationToken);
    }
}
