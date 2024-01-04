using API.Attributes;
using API.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using REFITS.Clients;
using AuthorizeAttribute = Microsoft.AspNetCore.Authorization.AuthorizeAttribute;

namespace API.Controllers;

[Route("api/[controller]")]
[ServiceFilter(typeof(LogActionFilter))]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ValidateToken]
public class HelperController : Controller
{
    private readonly IToDoClient _toDoClient;

    public HelperController(IToDoClient toDoClient)
    {
        _toDoClient = toDoClient;
    }

    [HttpGet("test")]
    public async Task<IActionResult> Get()
    {
        var response = await _toDoClient.Get();
        return Ok(response);
    }
}