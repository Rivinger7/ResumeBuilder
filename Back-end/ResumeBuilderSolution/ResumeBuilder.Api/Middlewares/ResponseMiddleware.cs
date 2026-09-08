using ResumeBuilder.Domain.Helpers;
using System.Text.Json;

namespace ResumeBuilder.Api.Middlewares;

internal sealed class ResponseMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context)
    {
        if (!context.Request.Path.StartsWithSegments("/api"))
        {
            await _next(context);
            return;
        }

        Stream originalBody = context.Response.Body;

        await using MemoryStream newBody = new();
        context.Response.Body = newBody;

        await _next(context);

        context.Response.Body = originalBody;

        if (context.Response.HasStarted)
        {
            return;
        }

        // Thêm điều kiện này — 204 không được phép có body
        if (context.Response.StatusCode == StatusCodes.Status204NoContent)
        {
            return;
        }

        // File nhị phân (PDF export, ảnh thumbnail, ...) — không decode UTF-8/rewrap JSON,
        // copy thẳng qua nguyên bytes. Chỉ áp dụng cho content-type file thật sự,
        // để không đổi hành vi của các response text/plain (vd. AuthenticationController
        // trả JWT dạng string thô) vốn vẫn cần được wrap như trước.
        string? contentType = context.Response.ContentType;
        bool isBinaryResponse = contentType is not null &&
            (contentType.Contains("application/pdf", StringComparison.OrdinalIgnoreCase) ||
             contentType.Contains("application/octet-stream", StringComparison.OrdinalIgnoreCase) ||
             contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase));
        if (isBinaryResponse)
        {
            newBody.Seek(0, SeekOrigin.Begin);
            await newBody.CopyToAsync(context.Response.Body);
            return;
        }

        newBody.Seek(0, SeekOrigin.Begin);
        string bodyText = await new StreamReader(newBody).ReadToEndAsync();

        if (context.Response.StatusCode >= 400)
        {
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(bodyText);
            return;
        }

        object? data = null;
        if (!string.IsNullOrWhiteSpace(bodyText))
        {
            try
            {
                data = JsonSerializer.Deserialize<object>(bodyText);
            }
            catch
            {
                data = bodyText;
            }
        }

        object response = new
        {
            context.Response.StatusCode,
            Message = "Success",
            Data = data,
            TraceId = context.TraceIdentifier,
            Path = context.Request.Path.Value ?? string.Empty,
            Timestamp = CustomTimeProvider.GetUtcPlus7TimeOffset()
        };

        string json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(json);
    }
}
