using API.ActionFilters;
using BLL.Abstract;
using DTO.Responses;
using DTO.Role;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using IResult = DTO.Responses.IResult;

namespace API.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
[ServiceFilter(typeof(LogActionFilter))]
[AllowAnonymous]
public class RoleController : Controller
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [SwaggerOperation(Summary = "get roles")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<List<RoleToListDto>>))]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var response = await _roleService.GetAsync();
        return Ok(response);
    }

    [SwaggerOperation(Summary = "get role")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<RoleToListDto>))]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var response = await _roleService.GetAsync(id);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "get role permissions")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<RoleToListDto>))]
    [HttpGet("{id}/permissions")]
    public async Task<IActionResult> GetRolePermissions([FromRoute] int id)
    {
        var response = await _roleService.GetPermissionsAsync(id);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "create role")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] RoleToAddDto dto)
    {
        var response = await _roleService.AddAsync(dto);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "update role")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] RoleToUpdateDto dto)
    {
        var response = await _roleService.UpdateAsync(id, dto);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "delete role")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var response = await _roleService.SoftDeleteAsync(id);
        return Ok(response);
    }
}