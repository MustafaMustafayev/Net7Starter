using API.ActionFilters;
using API.Attributes;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ServiceFilter(typeof(LogActionFilter))]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ValidateToken]
public class AntiForgeryController : Controller
{
    private readonly IAntiforgery _antiForgery;

    public AntiForgeryController(IAntiforgery antiForgery)
    {
        _antiForgery = antiForgery;
    }

    [HttpGet]
    [IgnoreAntiforgeryToken]
    public IActionResult GenerateAntiForgeryTokens()
    {
        var tokens = _antiForgery.GetAndStoreTokens(HttpContext);
        if (tokens.RequestToken is null) throw new Exception("Request tokens is null");
        Response.Cookies.Append("XSRF-REQUEST-TOKEN", tokens.RequestToken, new CookieOptions
        {
            HttpOnly = false
        });
        return Ok(tokens.RequestToken);
    }
}