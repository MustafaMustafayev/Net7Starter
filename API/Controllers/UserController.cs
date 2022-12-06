using BLL.Abstract;
using CORE.Config;
using DTO.Responses;
using DTO.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using IResult = DTO.Responses.IResult;

namespace API.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
[AllowAnonymous]
public class UserController : Controller
{
    private readonly ConfigSettings _configSettings;
    private readonly IUserService _userService;

    public UserController(IUserService userService, ConfigSettings configSettings)
    {
        _userService = userService;
        _configSettings = configSettings;
    }

    [SwaggerOperation(Summary = "get users as paginated list")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<List<UserToListDto>>))]
    [HttpGet("paginate")]
    public async Task<IActionResult> GetAsPaginated()
    {
        var pageIndex = Convert.ToInt32(HttpContext.Request.Headers[_configSettings.RequestSettings.PageIndex]);
        var pageSize = Convert.ToInt32(HttpContext.Request.Headers[_configSettings.RequestSettings.PageSize]);
        var response = await _userService.GetAsPaginatedListAsync(pageIndex, pageSize);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "get users")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<List<UserToListDto>>))]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var response = await _userService.GetAsync();
        return Ok(response);
    }

    [SwaggerOperation(Summary = "get user")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<UserToListDto>))]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var response = await _userService.GetAsync(id);
        return Ok(response);
    }

    [AllowAnonymous]
    [SwaggerOperation(Summary = "create user")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPost("register")]
    public async Task<IActionResult> Add([FromBody] UserToAddDto dto)
    {
        var response = await _userService.AddAsync(dto);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "update user")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UserToUpdateDto dto)
    {
        var response = await _userService.UpdateAsync(id, dto);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "delete user")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var response = await _userService.SoftDeleteAsync(id);
        return Ok(response);
    }
}