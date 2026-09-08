using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ResumeBuilder.Domain.Interfaces.Persistence;
using ResumeBuilder.Domain.Interfaces.Repositories;
using ResumeBuilder.Domain.Interfaces.Services;
using ResumeBuilder.Persistence.Context;
using ResumeBuilder.Persistence.Repositories;
using ResumeBuilder.Persistence.UnitOfWorks;

namespace ResumeBuilder.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        #region Database Connection
        string host = Environment.GetEnvironmentVariable("DB_HOST")
            ?? throw new Exception("DB_HOST is missing.");

        string port = Environment.GetEnvironmentVariable("DB_PORT")
            ?? throw new Exception("DB_PORT is missing.");

        string database = Environment.GetEnvironmentVariable("DB_NAME")
            ?? throw new Exception("DB_NAME is missing.");

        string username = Environment.GetEnvironmentVariable("DB_USERNAME")
            ?? throw new Exception("DB_USERNAME is missing.");

        string password = Environment.GetEnvironmentVariable("DB_PASSWORD")
            ?? throw new Exception("DB_PASSWORD is missing.");

        string connectionString =
            $"Host={host};Port={port};Database={database};Username={username};Password={password}";

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
        #endregion

        #region Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IResumeRepository, ResumeRepository>();
        services.AddScoped<IResumeSettingRepository, ResumeSettingRepository>();
        services.AddScoped<IResumeSectionRepository, ResumeSectionRepository>();
        services.AddScoped<ICertificateEntryRepository, CertificateEntryRepository>();
        services.AddScoped<IEducationEntryRepository, EducationEntryRepository>();
        services.AddScoped<IExperienceEntryRepository, ExperienceEntryRepository>();
        services.AddScoped<ILanguageEntryRepository, LanguageEntryRepository>();
        services.AddScoped<IObjectiveEntryRepository, ObjectiveEntryRepository>();
        services.AddScoped<IPersonalInformationEntryRepository, PersonalInformationEntryRepository>();
        services.AddScoped<IProjectEntryRepository, ProjectEntryRepository>();
        services.AddScoped<ISummaryEntryRepository, SummaryEntryRepository>();
        services.AddScoped<ISkillEntryRepository, SkillEntryRepository>();
        #endregion

        #region Services
        services.AddScoped<IDisplayOrderService, DisplayOrderService>();
        #endregion

        #region Unit Of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        #endregion

        return services;
    }
}