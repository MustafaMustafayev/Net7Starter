using API.ActionFilters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
[ServiceFilter(typeof(LogActionFilter))]
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