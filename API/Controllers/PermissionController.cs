using API.ActionFilters;
using API.Attributes;
using BLL.Abstract;
using CORE.Config;
using DTO.Permission;
using DTO.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers;

[Route("api/[controller]")]
[ServiceFilter(typeof(LogActionFilter))]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ValidateToken]
public class PermissionController : Controller
{
    private readonly ConfigSettings _configSettings;
    private readonly IPermissionService _permissionService;

    public PermissionController(IPermissionService permissionService, ConfigSettings configSettings)
    {
        _permissionService = permissionService;
        _configSettings = configSettings;
    }

    [SwaggerOperation(Summary = "get perrmissions as paginated list")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<List<PermissionToListDto>>))]
    [HttpGet("paginate")]
    public async Task<IActionResult> GetAsPaginated()
    {
        var pageIndex =
            Convert.ToInt32(HttpContext.Request.Headers[_configSettings.RequestSettings.PageIndex]);
        var pageSize =
            Convert.ToInt32(HttpContext.Request.Headers[_configSettings.RequestSettings.PageSize]);
        var response = await _permissionService.GetAsPaginatedListAsync(pageIndex, pageSize);

        return Ok(response);
    }

    [SwaggerOperation(Summary = "get permissions")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<List<PermissionToListDto>>))]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var response = await _permissionService.GetAsync();
        return Ok(response);
    }

    [SwaggerOperation(Summary = "get permission by id")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<PermissionToListDto>))]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var response = await _permissionService.GetAsync(id);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "create permission")]
    [HttpPost("register")]
    public async Task<IActionResult> Add([FromBody] PermissionToAddDto dto)
    {
        var response = await _permissionService.AddAsync(dto);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "update permission")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id,
        [FromBody] PermissionToUpdateDto dto)
    {
        var response = await _permissionService.UpdateAsync(id, dto);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "delete permission")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var response = await _permissionService.SoftDeleteAsync(id);
        return Ok(response);
    }
}