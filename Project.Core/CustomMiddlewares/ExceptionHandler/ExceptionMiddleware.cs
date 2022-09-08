using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Project.Core.CustomMiddlewares.Translation;
using Project.Core.Logging;
using Project.DTO.DTOs.Responses;
using Sentry;

// using Sentry;
namespace Project.Core.CustomMiddlewares.ExceptionHandler;

public class ExceptionMiddleware
{
    private readonly ILoggerManager _logger;
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next, ILoggerManager logger)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (AccessViolationException avEx)
        {
            _logger.LogError($"A new violation exception has been thrown: {avEx}");

            SentrySdk.CaptureException(avEx);
            await HandleExceptionAsync(httpContext, avEx);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong: {ex}");

            var sId = SentrySdk.CaptureException(ex);
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        var message = exception switch
        {
            AccessViolationException => "Access violation error from the custom middleware",
            _ => "Internal Server Error from the custom middleware."
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(
                new ErrorDataResult<Result>(Localization.Translate(Messages.GeneralError))));
    }
}