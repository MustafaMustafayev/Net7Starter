using CORE.Constants;
using System.Globalization;

namespace API.Middlewares;

public class LocalizationMiddleware
{
    private readonly RequestDelegate _next;

    public LocalizationMiddleware(RequestDelegate context)
    {
        _next = context;
    }

    public async Task Invoke(HttpContext context)
    {
        var requestLang = context.Request.Headers[LocalizationConstants.LANG_HEADER_NAME].ToString();

        var threadLang = requestLang switch
        {
            LocalizationConstants.LANG_HEADER_AZ => "az-Latn",
            LocalizationConstants.LANG_HEADER_EN => "en-GB",
            LocalizationConstants.LANG_HEADER_RU => "ru-RU",

            _ => "az-Latn"
        };

        Thread.CurrentThread.CurrentCulture = new CultureInfo(threadLang);
        Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

        context.Items["ClientLang"] = threadLang;
        context.Items["ClientCulture"] = Thread.CurrentThread.CurrentUICulture.Name;

        LocalizationConstants.CurrentLang = requestLang ?? LocalizationConstants.DefaultLang;

        await _next.Invoke(context);
    }
}