using API.ActionFilters;
using API.Attributes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers;

[Route("api/[controller]")]
[ServiceFilter(typeof(LogActionFilter))]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ValidateToken]
public class HelperController : Controller
{
    [SwaggerOperation(Summary = "search filter keys")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(HashSet<string>))]
    [HttpGet("searchFilterKeys")]
    public IActionResult GetFilterKeys()
    {
        //IEnumerable<string> filterKeys = _utilService.GetFilterKeys();
        return Ok(null);
    }
}