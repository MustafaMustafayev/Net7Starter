using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.API.ActionFilters;
using Project.BLL.Abstract;
using Project.Core.Abstract;
using Project.Core.Helper;
using Project.Core.Middlewares.Translation;
using Project.DTO.Auth;
using Project.DTO.Responses;
using Swashbuckle.AspNetCore.Annotations;
using IResult = Project.DTO.Responses.IResult;

namespace Project.API.Controllers;

[Route("api/[controller]")]
[ServiceFilter(typeof(LogActionFilter))]
[Authorize]
public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly ConfigSettings _configSettings;
    private readonly IUtilService _utilService;

    public AuthController(IAuthService authService, ConfigSettings configSettings, IUtilService utilService)
    {
        _authService = authService;
        _configSettings = configSettings;
        _utilService = utilService;
    }

    [SwaggerOperation(Summary = "login")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<LoginResponseDto>))]
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto request)
    {
        var userSalt = await _authService.GetUserSaltAsync(request.Email);

        if (string.IsNullOrEmpty(userSalt))
            return Ok(new ErrorDataResult<Result>(Localization.Translate(Messages.InvalidUserCredentials)));

        request.Password = SecurityHelper.HashPassword(request.Password, userSalt);

        var userDto = await _authService.LoginAsync(request);
        if (!userDto.Success) return Ok(userDto);

        var securityHelper = new SecurityHelper(_configSettings);

        var expireDate = DateTime.Now.AddHours(_configSettings.AuthSettings.TokenExpirationTimeInHours);

        var loginResponseDto = new LoginResponseDto
        {
            Token = securityHelper.CreateTokenForUser(userDto.Data, expireDate),
            User = userDto.Data,
            TokenExpireDate = expireDate
        };

        _utilService.AddTokenToCache(loginResponseDto.Token, expireDate);

        return Ok(new SuccessDataResult<LoginResponseDto>(loginResponseDto));
    }

    [SwaggerOperation(Summary = "send email for reset password")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpGet("verificationCode")]
    [AllowAnonymous]
    public IActionResult SendVerificationCode([FromQuery] string email)
    {
        return Ok(_authService.SendVerificationCodeToEmailAsync(email));
    }

    [SwaggerOperation(Summary = "reset password")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPost("resetPassword")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto request)
    {
        return Ok(await _authService.ResetPasswordAsync(request));
    }

    [SwaggerOperation(Summary = "login by token")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<LoginResponseDto>))]
    [HttpGet("loginByToken")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginByToken()
    {
        if (string.IsNullOrEmpty(HttpContext.Request.Headers.Authorization))
            return Unauthorized(new ErrorResult(Localization.Translate(Messages.CanNotFoundUserIdInYourAccessToken)));
        var userDto = await _authService.LoginByTokenAsync(HttpContext.Request.Headers.Authorization!);
        if (!userDto.Success) return BadRequest(userDto);

        var securityHelper = new SecurityHelper(_configSettings);

        var expireDate = DateTime.Now.AddHours(_configSettings.AuthSettings.TokenExpirationTimeInHours);

        var loginResponseDto = new LoginResponseDto
        {
            Token = securityHelper.CreateTokenForUser(userDto.Data, expireDate),
            User = userDto.Data,
            TokenExpireDate = expireDate
        };

        _utilService.AddTokenToCache(loginResponseDto.Token, expireDate);

        return Ok(new SuccessDataResult<LoginResponseDto>(loginResponseDto));
    }
}