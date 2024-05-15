using API.Attributes;
using API.Filters;
using BLL.Abstract;
using BLL.Concrete;
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
public class PermissionsController : Controller
{
    private readonly IPermissionService _permissionService;

    public PermissionsController(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    [SwaggerOperation(Summary = "get permissions as paginated list")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<List<PermissionResponseDto>>))]
    [HttpGet("{pageIndex}/{pageSize}")]
    public async Task<IActionResult> GetAsPaginated([FromRoute] int pageIndex, int pageSize)
    {
        var response = await _permissionService.GetAsPaginatedListAsync(pageIndex, pageSize);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "get permissions")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<List<PermissionResponseDto>>))]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var response = await _permissionService.GetAsync();
        return Ok(response);
    }

    [SwaggerOperation(Summary = "get permission by id")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<PermissionResponseDto>))]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var response = await _permissionService.GetAsync(id);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "create permission")]
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] PermissionCreateRequestDto dto)
    {
        var response = await _permissionService.AddAsync(dto);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "update permission")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id,
        [FromBody] PermissionUpdateRequestDto dto)
    {
        var response = await _permissionService.UpdateAsync(id, dto);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "delete permission")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var response = await _permissionService.SoftDeleteAsync(id);
        return Ok(response);
    }
}