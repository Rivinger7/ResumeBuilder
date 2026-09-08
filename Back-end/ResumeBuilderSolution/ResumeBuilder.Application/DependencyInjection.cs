using FluentValidation;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using ResumeBuilder.Application.Behaviors;
using System.Reflection;

namespace ResumeBuilder.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            cfg.LicenseKey = Environment.GetEnvironmentVariable("LUCKY_PENNY_API_KEY");
        });

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        #region Mapster
        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
        #endregion

        return services;
    }
}