using BLL.Abstract;
using CORE.Abstract;
using CORE.Config;
using CORE.Localization;
using CORE.Logging;
using DTO.ErrorLog;
using DTO.Responses;
using Microsoft.AspNetCore.Http.Features;
using Sentry;
using System.Net;
using System.Text.Json;

namespace API.Middlewares;

public class ExceptionMiddleware
{
    private readonly ConfigSettings _config;
    private readonly IWebHostEnvironment _env;
    private readonly ILoggerManager _logger;
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    public ExceptionMiddleware(RequestDelegate next, ILoggerManager logger, IWebHostEnvironment env,
        ConfigSettings config, IServiceScopeFactory serviceScopeFactory)
    {
        _config = config;
        _logger = logger;
        _env = env;
        _next = next;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong: {ex}");
            await LogErrorAsync(httpContext, ex);
            if (_config.SentrySettings.IsEnabled)
            {
                SentrySdk.CaptureException(ex);
            }

            //  if (_env.IsDevelopment()) throw;
            await HandleExceptionAsync(httpContext);
        }
    }

    private async Task LogErrorAsync(HttpContext httpContext, Exception ex)
    {
        using IServiceScope scope = _serviceScopeFactory.CreateScope();
        IUtilService _utilService = scope.ServiceProvider.GetRequiredService<IUtilService>();
        IErrorLogService _errorLogService = scope.ServiceProvider.GetRequiredService<IErrorLogService>();

        var traceIdentifier = httpContext.TraceIdentifier;
        var clientIp = httpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress?.ToString();
        var path = httpContext.Request.Path;
        string stackTrace = ex.StackTrace != null && ex.StackTrace.Length > 2000 ? ex.StackTrace[..2000] : ex.StackTrace;
        var token = string.Empty;
        Guid? userId = null;
        var authHeaderName = _config.AuthSettings.HeaderName;

        if (!string.IsNullOrEmpty(httpContext.Request.Headers[authHeaderName]) &&
            httpContext.Request.Headers[authHeaderName].ToString().Length > 7)
        {
            token = httpContext.Request.Headers[authHeaderName].ToString();
            userId = !string.IsNullOrEmpty(token)
                ? _utilService.GetUserIdFromToken()
                : null;
        }

        ErrorLogCreateDto errorLogToAddDto = new()
        {
            AccessToken = token,
            UserId = userId,
            Path = path,
            Ip = clientIp,
            ErrorMessage = ex.Message,
            StackTrace = stackTrace
        };
        await _errorLogService.AddAsync(errorLogToAddDto);
    }

    private static async Task HandleExceptionAsync(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        var response = new ErrorResult(Messages.GeneralError.Translate());
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}