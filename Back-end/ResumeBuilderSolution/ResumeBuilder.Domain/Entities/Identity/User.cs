using ResumeBuilder.Domain.Common;
using ResumeBuilder.Domain.Entities.Resumes;
using ResumeBuilder.Domain.Enums;

namespace ResumeBuilder.Domain.Entities.Identity;

public sealed class User : AuditableEntity
{
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string? AvatarUrl { get; set; }
    public string? PhoneNumber { get; set; }
    public UserRole Role { get; set; }

    public ICollection<Resume>? Resumes { get; set; }
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
}
