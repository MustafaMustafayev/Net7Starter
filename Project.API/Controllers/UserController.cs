using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.Abstract;
using Project.Core.Config;
using Project.DTO.Responses;
using Project.DTO.User;
using Swashbuckle.AspNetCore.Annotations;
using IResult = Project.DTO.Responses.IResult;

namespace Project.API.Controllers;

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
    [HttpGet("getAsPaginatedList")]
    public async Task<IActionResult> GetAsPaginated()
    {
        var pageIndex = Convert.ToInt32(HttpContext.Request.Headers[_configSettings.RequestSettings.PageIndex]);
        var pageSize = Convert.ToInt32(HttpContext.Request.Headers[_configSettings.RequestSettings.PageSize]);
        return Ok(await _userService.GetAsPaginatedListAsync(pageIndex, pageSize));
    }

    [SwaggerOperation(Summary = "get users")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<List<UserToListDto>>))]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _userService.GetAsync());
    }

    [SwaggerOperation(Summary = "get user")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<UserToListDto>))]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        return Ok(await _userService.GetAsync(id));
    }

    [AllowAnonymous]
    [SwaggerOperation(Summary = "create user")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPost("register")]
    public async Task<IActionResult> Add([FromBody] UserToAddDto dto)
    {
        return Ok(await _userService.AddAsync(dto));
    }

    [SwaggerOperation(Summary = "update user")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UserToUpdateDto dto)
    {
        return Ok(await _userService.UpdateAsync(dto));
    }

    [SwaggerOperation(Summary = "delete user")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        await _userService.DeleteAsync(id);
        return Ok(new SuccessDataResult<Result>());
    }
}