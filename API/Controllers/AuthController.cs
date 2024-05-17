using API.Attributes;
using API.Filters;
using BLL.Abstract;
using BLL.Helpers;
using CORE.Abstract;
using CORE.Config;
using CORE.Localization;
using DTO.Auth;
using DTO.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Swashbuckle.AspNetCore.Annotations;
using IResult = DTO.Responses.IResult;
using Result = DTO.Responses.Result;

namespace API.Controllers;

[Route("api/[controller]")]
[ServiceFilter(typeof(LogActionFilter))]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AuthController(
    IAuthService authService,
    ConfigSettings configSettings,
    IUtilService utilService,
    ITokenService tokenService) : Controller
{
    private readonly IAuthService _authService = authService;
    private readonly ConfigSettings _configSettings = configSettings;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IUtilService _utilService = utilService;

    [SwaggerOperation(Summary = "login")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<LoginResponseDto>))]
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var userSalt = await _authService.GetUserSaltAsync(request.Email);

        if (string.IsNullOrEmpty(userSalt))
        {
            return Ok(new ErrorDataResult<Result>(EMessages.InvalidUserCredentials.Translate()));
        }

        request = request with { Password = SecurityHelper.HashPassword(request.Password, userSalt) };

        var loginResult = await _authService.LoginAsync(request);
        if (!loginResult.Success)
        {
            return Unauthorized(loginResult);
        }

        var response = await _tokenService.CreateTokenAsync(loginResult.Data!);

        return Ok(response);
    }

    [SwaggerOperation(Summary = "refesh access token")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<LoginResponseDto>))]
    [ValidateToken]
    [HttpGet("refresh/token")]
    public async Task<IActionResult> RefreshToken()
    {
        var jwtToken = _utilService.TrimToken(HttpContext.Request.Headers[_configSettings.AuthSettings.HeaderName]!);

        string refreshToken = HttpContext.Request.Headers[_configSettings.AuthSettings.RefreshTokenHeaderName]!;

        var tokenResponse = await _tokenService.GetAsync(jwtToken, refreshToken);
        if (tokenResponse.Success)
        {
            await _tokenService.SoftDeleteAsync(tokenResponse.Data!.Id);
            var response = await _tokenService.CreateTokenAsync(tokenResponse.Data.User);
            return Ok(response);
        }

        return Unauthorized();
    }

    [SwaggerOperation(Summary = "send email for reset password")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPost("verificationCode")]
    [AllowAnonymous]
    public IActionResult SendVerificationCode([FromBody] SendVerificationCodeRequestDto dto)
    {
        var response = _authService.SendVerificationCodeToEmailAsync(dto.Email);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "reset password")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPost("resetPassword")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto request)
    {
        var response = await _authService.ResetPasswordAsync(request);
        var userId = _utilService.GetUserIdFromToken();
        await _authService.LogoutRemovedUserAsync(userId!.Value);

        return Ok(response);
    }

    [SwaggerOperation(Summary = "login by token")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<LoginResponseDto>))]
    [ValidateToken]
    [HttpGet("loginByToken")]
    public async Task<IActionResult> LoginByToken()
    {
        if (string.IsNullOrEmpty(HttpContext.Request.Headers.Authorization))
        {
            return Unauthorized(new ErrorResult(EMessages.CanNotFoundUserIdInYourAccessToken.Translate()));
        }

        var loginByTokenResponse = await _authService.LoginByTokenAsync();
        if (!loginByTokenResponse.Success)
        {
            return BadRequest(loginByTokenResponse.Data);
        }

        var response = await _tokenService.CreateTokenAsync(loginByTokenResponse.Data!);

        return Ok(response);
    }

    [SwaggerOperation(Summary = "logout")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [ValidateToken]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var accessToken = _utilService.TrimToken(_utilService.GetTokenString()!);
        var response = await _authService.LogoutAsync(accessToken);

        return Ok(response);
    }
}