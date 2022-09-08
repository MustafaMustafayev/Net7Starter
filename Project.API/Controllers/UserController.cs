using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.Abstract;
using Project.Core.Helper;
using Project.DTO.DTOs.AuthDTOs;
using Project.DTO.DTOs.Responses;
using Project.DTO.DTOs.UserDTOs;
using Swashbuckle.AspNetCore.Annotations;

namespace Project.API.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
//[ServiceFilter(typeof(LogActionFilter))]
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

    [SwaggerOperation(Summary = "get users")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(List<UserToListDTO>))]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var pageIndex = Convert.ToInt32(HttpContext.Request.Headers[_configSettings.RequestSettings.PageIndex]);
        var pageSize = Convert.ToInt32(HttpContext.Request.Headers[_configSettings.RequestSettings.PageSize]);
        return Ok(await _userService.GetAsync());
    }

    [SwaggerOperation(Summary = "get user")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(UserToListDTO))]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        return Ok(await _userService.GetAsync(id));
    }

    [AllowAnonymous]
    [SwaggerOperation(Summary = "create user")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(UserToListDTO))]
    [HttpPost("register")]
    public async Task<IActionResult> Add([FromBody] UserToAddDTO dto)
    {
        return Ok(await _userService.AddAsync(dto));
    }

    [SwaggerOperation(Summary = "update user")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(UserToListDTO))]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UserToUpdateDTO dto)
    {
        return Ok(await _userService.UpdateAsync(id, dto));
    }

    [SwaggerOperation(Summary = "delete user")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(void))]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        await _userService.DeleteAsync(id);
        return Ok(new SuccessDataResult<Result>());
    }

    [SwaggerOperation(Summary = "reset password")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(void))]
    [HttpPatch("resetPassword")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDto)
    {
        await _userService.ResetPasswordAsync(resetPasswordDto);
        return Ok();
    }
}