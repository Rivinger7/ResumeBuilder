using ResumeBuilder.Api.Middlewares;

namespace ResumeBuilder.Api.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
        return app;
    }

    public static IApplicationBuilder UseResponseWrapperMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ResponseMiddleware>();
        return app;
    }
}
