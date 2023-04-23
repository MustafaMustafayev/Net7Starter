using API.ActionFilters;
using API.Attributes;
using BLL.Abstract;
using CORE.Abstract;
using CORE.Localization;
using DTO.Responses;
using DTO.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using IResult = DTO.Responses.IResult;

namespace API.Controllers;

[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UserController : Controller
{
    private readonly IWebHostEnvironment _environment;
    private readonly IUserService _userService;
    private readonly IUtilService _utilService;

    public UserController(IUserService userService, IWebHostEnvironment environment,
        IUtilService utilService)
    {
        _userService = userService;
        _environment = environment;
        _utilService = utilService;
    }

    [SwaggerOperation(Summary = "get users as paginated list")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<List<UserToListDto>>))]
    [HttpGet("paginate")]
    [ValidateToken]
    [ServiceFilter(typeof(LogActionFilter))]
    public async Task<IActionResult> GetAsPaginated()
    {
        var response = await _userService.GetAsPaginatedListAsync();
        return Ok(response);
    }

    [SwaggerOperation(Summary = "get users")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<List<UserToListDto>>))]
    [HttpGet]
    [ValidateToken]
    [ServiceFilter(typeof(LogActionFilter))]
    public async Task<IActionResult> Get()
    {
        var response = await _userService.GetAsync();
        return Ok(response);
    }

    [ServiceFilter(typeof(LogActionFilter))]
    [SwaggerOperation(Summary = "get profile info")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<List<UserToListDto>>))]
    [HttpGet("profile")]
    public async Task<IActionResult> GetProfileInfo()
    {
        var userId = _utilService.GetUserIdFromToken();
        if (userId is null)
            return Unauthorized(new ErrorResult(Messages.CanNotFoundUserIdInYourAccessToken.Translate()));

        var response = await _userService.GetAsync(userId.Value);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "get user")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<UserToListDto>))]
    [HttpGet("{id}")]
    [ValidateToken]
    [ServiceFilter(typeof(LogActionFilter))]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var response = await _userService.GetAsync(id);
        return Ok(response);
    }

    [AllowAnonymous]
    [SwaggerOperation(Summary = "create user")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPost("register")]
    [ServiceFilter(typeof(LogActionFilter))]
    public async Task<IActionResult> Add([FromBody] UserToAddDto dto)
    {
        var response = await _userService.AddAsync(dto);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "update user")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPut("{id}")]
    [ValidateToken]
    [ServiceFilter(typeof(LogActionFilter))]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UserToUpdateDto dto)
    {
        var response = await _userService.UpdateAsync(id, dto);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "delete user")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpDelete("{id}")]
    [ValidateToken]
    [ServiceFilter(typeof(LogActionFilter))]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var response = await _userService.SoftDeleteAsync(id);
        return Ok(response);
    }
}