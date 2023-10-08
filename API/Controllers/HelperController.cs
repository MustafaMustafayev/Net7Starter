using API.Attributes;
using API.Filters;
using BLL.External.Clients;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ServiceFilter(typeof(LogActionFilter))]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ValidateToken]
public class HelperController : Controller
{
    private readonly IAntiforgery _antiForgery;
    private readonly IStudentClient _studentClient;

    public HelperController(IStudentClient studentClient, IAntiforgery antiForgery)
    {
        _studentClient = studentClient;
        _antiForgery = antiForgery;
    }

    [HttpGet("test/studentapi/")]
    public async Task<IActionResult> Get()
    {
        var response = await _studentClient.GetAsync(24);
        return Ok(response);
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