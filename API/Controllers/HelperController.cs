using API.Attributes;
using API.Filters;
using BLL.External.Clients;
using CORE.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Refit;
using AuthorizeAttribute = Microsoft.AspNetCore.Authorization.AuthorizeAttribute;

namespace API.Controllers;

[Route("api/[controller]")]
[ServiceFilter(typeof(LogActionFilter))]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ValidateToken]
public class HelperController : Controller
{
    private readonly IPetStoreClient _petStoreClient;

    public HelperController(IPetStoreClient petStoreClient)
    {
        _petStoreClient = petStoreClient;
    }

    [HttpGet("test")]
    public async Task<IActionResult> Get()
    {
        var response = await _petStoreClient.GetOrderById(24);
        return Ok(response);
    }
}