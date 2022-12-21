
using CORE.Constants;
using Microsoft.AspNetCore.Antiforgery;

namespace API.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ValidateAntiForgeryTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private  readonly IAntiforgery _antiForgery;

        public ValidateAntiForgeryTokenMiddleware(RequestDelegate next, IAntiforgery antiforgery)
        {
            _next = next;
            _antiForgery = antiforgery;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (Constants.AntiForgeryTokenMethodTypes.Contains(httpContext.Request.Method.ToLower()))
            {
                await _antiForgery.ValidateRequestAsync(httpContext);
            }
            await _next.Invoke(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ValidateAntiForgeryTokenMiddlewareExtensions
    {
        public static IApplicationBuilder UseMiddlewareClassTemplate(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ValidateAntiForgeryTokenMiddleware>();
        }
    }
}

