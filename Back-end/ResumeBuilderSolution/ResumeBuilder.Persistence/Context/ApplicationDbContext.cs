using Microsoft.EntityFrameworkCore;
using ResumeBuilder.Domain.Entities.Identity;
using ResumeBuilder.Domain.Entities.Resumes;
using ResumeBuilder.Domain.Entities.Resumes.Entries;

namespace ResumeBuilder.Persistence.Context;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public DbSet<Resume> Resumes => Set<Resume>();
    public DbSet<ResumeSection> ResumeSections => Set<ResumeSection>();
    public DbSet<ResumeSetting> ResumeSettings => Set<ResumeSetting>();

    public DbSet<CertificateEntry> CertificateEntries => Set<CertificateEntry>();
    public DbSet<EducationEntry> EducationEntries => Set<EducationEntry>();
    public DbSet<ExperienceEntry> ExperienceEntries => Set<ExperienceEntry>();
    public DbSet<LanguageEntry> LanguageEntries => Set<LanguageEntry>();
    public DbSet<ObjectiveEntry> ObjectiveEntries => Set<ObjectiveEntry>();
    public DbSet<PersonalInformationEntry> PersonalInformationEntries => Set<PersonalInformationEntry>();
    public DbSet<ProjectEntry> ProjectEntries => Set<ProjectEntry>();
    public DbSet<SummaryEntry> SummaryEntries => Set<SummaryEntry>();
    public DbSet<SkillEntry> SkillEntries => Set<SkillEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}