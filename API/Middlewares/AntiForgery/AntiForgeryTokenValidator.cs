using Microsoft.AspNetCore.Antiforgery;

namespace API.Middlewares;

public class AntiForgeryTokenValidator
{
    private readonly RequestDelegate _next;

    public AntiForgeryTokenValidator(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext, IAntiforgery antiforgery)
    {
        var avaliableMethodTypes = new List<string> { "post", "put", "delete" };
        if (!httpContext.Request.Path.Value!.Contains("api/AntiForgery") &&
            avaliableMethodTypes.Contains(httpContext.Request.Method.ToLower()))
            await antiforgery.ValidateRequestAsync(httpContext);
        await _next.Invoke(httpContext);
    }
}