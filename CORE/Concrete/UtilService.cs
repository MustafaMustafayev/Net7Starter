using CORE.Abstract;
using CORE.Config;
using DTO.Helper;
using ENTITIES.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace CORE.Concrete;

public class UtilService : IUtilService
{
    private readonly ConfigSettings _config;
    private readonly IHttpContextAccessor _context;
    private readonly IWebHostEnvironment _environment;

    public UtilService(ConfigSettings config, IHttpContextAccessor context, IWebHostEnvironment environment)
    {
        _config = config;
        _context = context;
        _environment = environment;
    }

    public string? GetTokenString()
    {
        return _context.HttpContext?.Request.Headers[_config.AuthSettings.HeaderName].ToString();
    }

    public int? GetUserIdFromToken()
    {
        var token = GetJwtSecurityToken();
        if (token == null) return null;
        var userId = Decrypt(token.Claims.First(c => c.Type == _config.AuthSettings.TokenUserIdKey).Value);
        return Convert.ToInt32(userId);
    }

    public int? GetCompanyIdFromToken()
    {
        var token = GetJwtSecurityToken();
        if (token is null) return null;

        var companyIdClaim = token.Claims.First(c => c.Type == _config.AuthSettings.TokenCompanyIdKey);

        if (companyIdClaim is null || string.IsNullOrEmpty(companyIdClaim.Value)) return null;

        return Convert.ToInt32(companyIdClaim.Value);
    }

    public bool IsValidToken()
    {
        var tokenString = GetTokenString();

        if (string.IsNullOrEmpty(tokenString) || tokenString.Length < 7) return false;

        var tokenHandler = new JwtSecurityTokenHandler();
        var secretKey = Encoding.ASCII.GetBytes(_config.AuthSettings.SecretKey);
        try
        {
            tokenHandler.ValidateToken(tokenString[7..], new TokenValidationParameters
            {
                ValidateIssuerSigningKey = false,
                IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }

    public string TrimToken(string? jwtToken)
    {
        if (string.IsNullOrEmpty(jwtToken) || jwtToken.Length < 7) throw new Exception();

        return jwtToken[7..];
    }

    public string Encrypt(string value)
    {
        var key = _config.CryptographySettings.KeyBase64;
        var privatekey = _config.CryptographySettings.VBase64;
        var privatekeyByte = Encoding.UTF8.GetBytes(privatekey);
        var keybyte = Encoding.UTF8.GetBytes(key);
        SymmetricAlgorithm algorithm = Aes.Create();
        var transform = algorithm.CreateEncryptor(keybyte, privatekeyByte);
        var inputbuffer = Encoding.Unicode.GetBytes(value);
        var outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
        return Convert.ToBase64String(outputBuffer);
    }

    public string Decrypt(string value)
    {
        var key = _config.CryptographySettings.KeyBase64;
        var privatekey = _config.CryptographySettings.VBase64;
        var privatekeyByte = Encoding.UTF8.GetBytes(privatekey);
        var keybyte = Encoding.UTF8.GetBytes(key);
        SymmetricAlgorithm algorithm = Aes.Create();
        var transform = algorithm.CreateDecryptor(keybyte, privatekeyByte);
        var inputbuffer = Convert.FromBase64String(value);
        var outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
        return Encoding.Unicode.GetString(outputBuffer);
    }

    public async Task SendMailAsync(string email, string message)
    {
        if (!string.IsNullOrEmpty(email) && email.Contains('@'))
        {
            var fromAddress = new MailAddress(_config.MailSettings.Address, _config.MailSettings.DisplayName);
            var toAddress = new MailAddress(email, email);

            var smtp = new SmtpClient
            {
                Host = _config.MailSettings.Host,
                Port = int.Parse(_config.MailSettings.Port),
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, _config.MailSettings.MailKey)
            };

            using var data = new MailMessage(fromAddress, toAddress)
            {
                Subject = _config.MailSettings.Subject,
                Body = message
            };

            await smtp.SendMailAsync(data);
        }
    }

    public PaginationDto GetPagination()
    {
        var pageIndex = Convert.ToInt32(_context.HttpContext?.Request.Headers[_config.RequestSettings.PageIndex]);
        var pageSize = Convert.ToInt32(_context.HttpContext?.Request.Headers[_config.RequestSettings.PageSize]);

        var dto = new PaginationDto
        {
            PageIndex = pageIndex,
            PageSize = pageSize
        };

        return dto;
    }

    public string CreateGuid()
    {
        return Guid.NewGuid().ToString();
    }

    public string? GetRoleFromToken(string? tokenString)
    {
        var token = GetJwtSecurityToken();
        if (token == null) return null;

        var roleIdClaim = token.Claims.First(c => c.Type == _config.AuthSettings.Role);

        if (roleIdClaim is null || string.IsNullOrEmpty(roleIdClaim.Value)) return null;

        return roleIdClaim.Value;
    }

    public string GetFolderName(FileType type)
    {
        return type switch
        {
            FileType.UserProfile => @"files\images\user_profile",
            FileType.OrganizationLogo => @"files\images\organization_logo",
            _ => "files/error"
        };
    }

    public string GetEnvFolderPath(string folderName)
    {
        return Path.Combine(_environment.WebRootPath, folderName);
    }

    private JwtSecurityToken? GetJwtSecurityToken()
    {
        var tokenString = GetTokenString();

        if (string.IsNullOrEmpty(tokenString)) return null;
        return !tokenString.Contains($"{_config.AuthSettings.TokenPrefix} ")
            ? null
            : new JwtSecurityToken(tokenString[7..]);
    }
}