using Microsoft.AspNetCore.Builder;

namespace Project.Core.CustomMiddlewares.ExceptionHandler;

public static class ConfigureExceptionMiddleware
{
    public static void ConfigureCustomExceptionMiddleware(this WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
    }
}