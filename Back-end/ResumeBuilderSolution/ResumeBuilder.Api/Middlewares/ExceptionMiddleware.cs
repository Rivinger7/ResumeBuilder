using Microsoft.AspNetCore.Mvc;
using ResumeBuilder.Domain.Exceptions;

namespace ResumeBuilder.Api.Middlewares;

internal sealed class ExceptionMiddleware(RequestDelegate next, IProblemDetailsService problemDetailsService)
{
    private readonly IProblemDetailsService _problemDetailsService = problemDetailsService;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        if (context.Response.HasStarted)
        {
            return; // Only logging here or just stop
        }

        ProblemDetails problem = exception switch
        {
            FluentValidation.ValidationException validation => CreateValidationProblem(validation),

            ConflictException => CreateProblem(context, StatusCodes.Status409Conflict, exception.Message),

            NotFoundException => CreateProblem(context, StatusCodes.Status404NotFound, exception.Message),

            UnauthorizedException => CreateProblem(context, StatusCodes.Status401Unauthorized, exception.Message),

            ForbiddenException => CreateProblem(context, StatusCodes.Status403Forbidden, exception.Message),

            BadRequestException => CreateProblem(context, StatusCodes.Status400BadRequest, exception.Message),

            _ => CreateProblem(context, StatusCodes.Status500InternalServerError, "Internal Server Error")
        };

        context.Response.StatusCode = problem.Status!.Value;

        await _problemDetailsService.WriteAsync(
            new ProblemDetailsContext
            {
                HttpContext = context,
                ProblemDetails = problem
            });
    }
    private static ProblemDetails CreateProblem(HttpContext context, int status, string title)
    {
        return new ProblemDetails
        {
            Status = status,
            Title = title,
            Type = $"https://httpstatuses.com/{status}",
            Instance = context.Request.Path
        };
    }
    private static ValidationProblemDetails CreateValidationProblem(FluentValidation.ValidationException exception)
    {
        Dictionary<string, string[]> errors = exception.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.ErrorMessage).ToArray());

        string detail = string.Join(" | ", exception.Errors.Select(x => x.ErrorMessage));

        return new ValidationProblemDetails(errors)
        {
            Title = "Validation Failed",
            Detail = detail,
            Status = StatusCodes.Status400BadRequest
        };
    }
}
