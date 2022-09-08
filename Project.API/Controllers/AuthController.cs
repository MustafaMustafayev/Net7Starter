using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Project.API.ActionFilters;
using Project.BLL.Abstract;
using Project.Core.Constants;
using Project.Core.Helper;
using Project.DTO.DTOs.AuthDTOs;
using Project.DTO.DTOs.Responses;
using Project.DTO.DTOs.UserDTOs;
using Swashbuckle.AspNetCore.Annotations;

namespace Project.API.Controllers;

[Route("api/[controller]")]
[ServiceFilter(typeof(LogActionFilter))]
public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly ConfigSettings _configSettings;
    private IUserService _userService;

    public AuthController(IAuthService authService, IUserService userService, ConfigSettings configSettings)
    {
        _authService = authService;
        _userService = userService;
        _configSettings = configSettings;
    }

    [SwaggerOperation(Summary = "login")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(LoginResponseDTO))]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
    {
        string userSalt = await _authService.GetUserSaltAsync(loginDto.PIN);
        if (string.IsNullOrEmpty(userSalt)) return Ok(new ErrorDataResult<Result>(Messages.InvalidModel));

        loginDto.Password = SecurityHelper.HashPassword(loginDto.Password, userSalt);
        IDataResult<UserToListDTO> userDto = await _authService.LoginAsync(loginDto);
        if (!userDto.Success) return Ok(userDto);

        DateTime expirationDate = DateTime.Now.AddDays(1);

        List<Claim> claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.NameIdentifier, userDto.Data.UserId.ToString()));
        claims.Add(new Claim(ClaimTypes.Name, userDto.Data.PIN));
        claims.Add(new Claim(ClaimTypes.Expiration, expirationDate.ToString()));

        //TODO add organization field to token body
        claims.Add(new Claim(_configSettings.AuthSettings.TokenCompanyIdKey, userDto.Data.OrganizationId.ToString()));

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configSettings.AuthSettings.SecretKey));
        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expirationDate,
            SigningCredentials = creds
        };
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        string tokenValue = tokenHandler.WriteToken(token);

        LoginResponseDTO loginResponseDto = new LoginResponseDTO
        {
            Token = tokenValue,
            User = userDto.Data
        };

        return Ok(new SuccessDataResult<LoginResponseDTO>(loginResponseDto));
    }

    [SwaggerOperation(Summary = "logout")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(void))]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        // implement logout logic due requirements
        return Ok();
    }
}