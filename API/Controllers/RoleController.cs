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
        return Ok(await _roleService.GetAsync());
    }

    [SwaggerOperation(Summary = "get role")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<RoleToListDto>))]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        return Ok(await _roleService.GetAsync(id));
    }

    [SwaggerOperation(Summary = "create role")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] RoleToAddDto role)
    {
        return Ok(await _roleService.AddAsync(role));
    }

    [SwaggerOperation(Summary = "update role")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] RoleToUpdateDto role)
    {
        return Ok(await _roleService.UpdateAsync(role));
    }

    [SwaggerOperation(Summary = "delete role")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        await _roleService.DeleteAsync(id);
        return Ok();
    }
}