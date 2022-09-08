using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.API.ActionFilters;
using Project.Core.Abstract;
using Swashbuckle.AspNetCore.Annotations;

namespace Project.API.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
[ServiceFilter(typeof(LogActionFilter))]
public class HelperController : Controller
{
    private readonly IUtilService _utilService;

    public HelperController(IUtilService utilService)
    {
        _utilService = utilService;
    }

    [SwaggerOperation(Summary = "search filter keys")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(HashSet<string>))]
    [HttpGet("searchFilterKeys")]
    public IActionResult GetFilterKeys()
    {
        IEnumerable<string> filterKeys = _utilService.GetFilterKeys();
        return Ok(filterKeys);
    }
}