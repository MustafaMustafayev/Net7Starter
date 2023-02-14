using CORE.Constants;
using Microsoft.AspNetCore.Antiforgery;

namespace API.Middlewares.AntiForgery;

// You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
public class ValidateAntiForgeryTokenMiddleware
{
    private readonly IAntiforgery _antiForgery;
    private readonly RequestDelegate _next;

    public ValidateAntiForgeryTokenMiddleware(RequestDelegate next, IAntiforgery antiforgery)
    {
        _next = next;
        _antiForgery = antiforgery;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        if (Constants.AntiForgeryTokenMethodTypes.Contains(httpContext.Request.Method.ToLower()))
            await _antiForgery.ValidateRequestAsync(httpContext);
        await _next.Invoke(httpContext);
    }
}