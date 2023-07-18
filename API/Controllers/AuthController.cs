using API.Attributes;
using API.Filters;
using BLL.Abstract;
using CORE.Abstract;
using CORE.Config;
using CORE.Helper;
using CORE.Localization;
using DAL.MongoDb;
using DTO.Auth;
using DTO.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using IResult = DTO.Responses.IResult;

namespace API.Controllers;

[Route("api/[controller]")]
[ServiceFilter(typeof(LogActionFilter))]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly ConfigSettings _configSettings;
    private readonly IMongoDbService _mongoDbService;
    private readonly ITokenService _tokenService;
    private readonly IUtilService _utilService;

    public AuthController(
        IAuthService authService,
        ConfigSettings configSettings,
        IUtilService utilService,
        ITokenService tokenService,
        IMongoDbService mongoDbService)
    {
        _authService = authService;
        _configSettings = configSettings;
        _utilService = utilService;
        _tokenService = tokenService;
        _mongoDbService = mongoDbService;
    }

    [SwaggerOperation(Summary = "login")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<LoginResponseDto>))]
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto request)
    {
        var userSalt = await _authService.GetUserSaltAsync(request.Email);

        if (string.IsNullOrEmpty(userSalt))
            return Ok(new ErrorDataResult<Result>(Messages.InvalidUserCredentials.Translate()));

        request = request with { Password = SecurityHelper.HashPassword(request.Password, userSalt) };

        var loginResult = await _authService.LoginAsync(request);
        if (!loginResult.Success) return Unauthorized(loginResult);

        var response = await _tokenService.CreateTokenAsync(loginResult.Data!);

        return Ok(response);
    }

    [SwaggerOperation(Summary = "send email for reset password")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpGet("verificationCode")]
    [AllowAnonymous]
    public IActionResult SendVerificationCode([FromQuery] string email)
    {
        return Ok(_authService.SendVerificationCodeToEmailAsync(email));
    }

    [SwaggerOperation(Summary = "refesh access token")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<LoginResponseDto>))]
    [ValidateToken]
    [HttpGet("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var jwtToken = _utilService.GetTokenStringFromHeader(HttpContext.Request.Headers[_configSettings.AuthSettings.HeaderName]!);
        string refreshToken = HttpContext.Request.Headers[_configSettings.AuthSettings.RefreshTokenHeaderName]!;

        var tokenResponse = await _tokenService.GetAsync(jwtToken, refreshToken);
        if (tokenResponse.Success)
        {
            await _tokenService.SoftDeleteAsync(tokenResponse.Data!.TokenId);
            var response = await _tokenService.CreateTokenAsync(tokenResponse.Data.User);
            return Ok(response);
        }

        return Unauthorized();
    }

    [SwaggerOperation(Summary = "reset password")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPost("resetPassword")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto request)
    {
        var response = await _authService.ResetPasswordAsync(request);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "login by token")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<LoginResponseDto>))]
    [ValidateToken]
    [HttpGet("loginByToken")]
    public async Task<IActionResult> LoginByToken()
    {
        if (string.IsNullOrEmpty(HttpContext.Request.Headers.Authorization))
            return Unauthorized(new ErrorResult(Messages.CanNotFoundUserIdInYourAccessToken.Translate()));

        var loginByTokenResponse = await _authService.LoginByTokenAsync();
        if (!loginByTokenResponse.Success) return BadRequest(loginByTokenResponse.Data);

        var response = await _tokenService.CreateTokenAsync(loginByTokenResponse.Data!);

        return Ok(response);
    }

    [SwaggerOperation(Summary = "logout")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var accessToken = _utilService.GetTokenStringFromHeader(_utilService.GetTokenString()!);
        var response = await _authService.LogoutAsync(accessToken);

        return Ok(response);
    }
}