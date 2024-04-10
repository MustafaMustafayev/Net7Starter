using API.Attributes;
using API.Filters;
using BLL.Abstract;
using DTO.Responses;
using DTO.Role;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using IResult = DTO.Responses.IResult;

namespace API.Controllers;

[Route("api/[controller]")]
[ServiceFilter(typeof(LogActionFilter))]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ValidateToken]
public class RolesController : Controller
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [SwaggerOperation(Summary = "get roles")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<List<RoleResponseDto>>))]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var response = await _roleService.GetAsync();
        return Ok(response);
    }

    [SwaggerOperation(Summary = "get role")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<RoleResponseDto>))]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var response = await _roleService.GetAsync(id);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "get role permissions")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<RoleResponseDto>))]
    [HttpGet("{id}/permissions")]
    public async Task<IActionResult> GetRolePermissions([FromRoute] Guid id)
    {
        var response = await _roleService.GetPermissionsAsync(id);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "create role")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] RoleCreateRequestDto dto)
    {
        var response = await _roleService.AddAsync(dto);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "update role")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] RoleUpdateRequestDto dto)
    {
        var response = await _roleService.UpdateAsync(id, dto);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "delete role")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var response = await _roleService.SoftDeleteAsync(id);
        return Ok(response);
    }
}