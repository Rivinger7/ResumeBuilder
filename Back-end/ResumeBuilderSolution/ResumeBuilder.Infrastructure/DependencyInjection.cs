using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using ResumeBuilder.Domain.Interfaces.Identity;
using ResumeBuilder.Domain.Interfaces.Services;
using ResumeBuilder.Infrastructure.Authentication;
using ResumeBuilder.Infrastructure.PDFs;
using ResumeBuilder.Infrastructure.Storage;
using System.Security.Claims;
using System.Text;

namespace ResumeBuilder.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IGoogleValidation, GoogleValidation>();

        #region Initialize Jwt params
        string issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? throw new Exception("JWT_ISSUER is missing");
        string audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? throw new Exception("JWT_AUDIENCE is missing");
        string secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? throw new Exception("SecretKey is missing");
        string expiration = Environment.GetEnvironmentVariable("JWT_EXPIRATION_MINUTES") ?? throw new Exception("JWT_EXPIRATION_MINUTES is missing");
        string googleClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID") ?? throw new Exception("GOOGLE_CLIENT_ID property is not set in environment or not found");
        string googleClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET") ?? throw new Exception("GOOGLE_CLIENT_SECRET property is not set in environment or not found");

        JwtOptions jwtOptions = new()
        {
            Issuer = issuer,
            Audience = audience,
            SecretKey = secretKey,
            ExpirationInMinutes = int.Parse(expiration),
            GoogleClientId = googleClientId,
            GoogleClientSecret = googleClientSecret
        };
        services.AddSingleton(jwtOptions);
        #endregion

        #region Register Jwt
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
        })
        //.AddGoogle(googleOptions =>
        //{
        //    googleOptions.ClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID") ?? throw new Exception("GOOGLE_CLIENT_ID property is not set in environment or not found");
        //    googleOptions.ClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET") ?? throw new Exception("GOOGLE_CLIENT_SECRET property is not set in environment or not found");

        //})
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                ClockSkew = TimeSpan.Zero,
                RoleClaimType = ClaimTypes.Role
            };

            options.MapInboundClaims = false;
        });

        services.AddAuthorization();
        #endregion

        #region QuestPDF
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
        services.AddScoped<IThumbnailGenerator>(serviceProvider =>
        {
            IHostEnvironment hostEnvironment = serviceProvider.GetRequiredService<IHostEnvironment>();
            string webRootPath = Path.Combine(hostEnvironment.ContentRootPath, "wwwroot");

            return new ThumnailGenerator(webRootPath);
        });
        services.AddScoped<IPdfExporter, PdfExporter>();
        services.AddScoped<IPhotoStorage>(serviceProvider =>
        {
            IHostEnvironment hostEnvironment = serviceProvider.GetRequiredService<IHostEnvironment>();
            string webRootPath = Path.Combine(hostEnvironment.ContentRootPath, "wwwroot");

            return new PhotoStorage(webRootPath);
        });

        string fontsPath = Path.Combine(AppContext.BaseDirectory, "wwwroot", "Fonts", "Source_Sans_3");
        if (Directory.Exists(fontsPath))
        {
            foreach (string fontFile in Directory.GetFiles(fontsPath, "*.ttf", SearchOption.AllDirectories))
            {
                using FileStream stream = File.OpenRead(fontFile);
                QuestPDF.Drawing.FontManager.RegisterFont(stream);
            }
        }
        #endregion

        return services;
    }
}
