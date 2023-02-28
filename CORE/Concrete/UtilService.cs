using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using CORE.Abstract;
using CORE.Config;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace CORE.Concrete;

public class UtilService : IUtilService
{
    private readonly ConfigSettings _configSettings;

    public UtilService(ConfigSettings configSettings)
    {
        _configSettings = configSettings;
    }

    public HttpContent GetHttpContentObject(object obj)
    {
        return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, GetContentType());
    }

    public int? GetUserIdFromToken(string? tokenString)
    {
        if (string.IsNullOrEmpty(tokenString)) return null;
        if (!tokenString.Contains($"{_configSettings.AuthSettings.TokenPrefix} ")) return null;

        var token = new JwtSecurityToken(tokenString[7..]);
        var userId = Decrypt(token.Claims.First(c => c.Type == _configSettings.AuthSettings.TokenUserIdKey).Value);
        return Convert.ToInt32(userId);
    }

    public int? GetCompanyIdFromToken(string? tokenString)
    {
        if (string.IsNullOrEmpty(tokenString)) return null;
        if (!tokenString.Contains($"{_configSettings.AuthSettings.TokenPrefix} ")) return null;

        var token = new JwtSecurityToken(tokenString[7..]);
        var companyIdClaim =
            token.Claims.First(c => c.Type == _configSettings.AuthSettings.TokenCompanyIdKey);

        if (companyIdClaim is null || string.IsNullOrEmpty(companyIdClaim.Value)) return null;

        return Convert.ToInt32(companyIdClaim.Value);
    }

    public bool IsValidToken(string tokenString)
    {
        if (string.IsNullOrEmpty(tokenString) || tokenString.Length < 7) return false;

        var tokenHandler = new JwtSecurityTokenHandler();
        var secretKey = Encoding.ASCII.GetBytes(_configSettings.AuthSettings.SecretKey);
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

    public string GetTokenStringFromHeader(string? jwtToken)
    {
        if (string.IsNullOrEmpty(jwtToken) || jwtToken.Length < 7) throw new Exception();

        return jwtToken[7..];
    }

    public string Encrypt(string value)
    {
        var _key = _configSettings.CryptographySettings.KeyBase64;
        var privatekey = _configSettings.CryptographySettings.VBase64;
        byte[] privatekeyByte = { };
        privatekeyByte = Encoding.UTF8.GetBytes(privatekey);
        byte[] _keybyte = { };
        _keybyte = Encoding.UTF8.GetBytes(_key);
        SymmetricAlgorithm algorithm = DES.Create();
        var transform = algorithm.CreateEncryptor(_keybyte, privatekeyByte);
        var inputbuffer = Encoding.Unicode.GetBytes(value);
        var outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
        return Convert.ToBase64String(outputBuffer);
    }

    public string Decrypt(string value)
    {
        var _key = _configSettings.CryptographySettings.KeyBase64;
        var privatekey = _configSettings.CryptographySettings.VBase64;
        byte[] privatekeyByte = { };
        privatekeyByte = Encoding.UTF8.GetBytes(privatekey);
        byte[] _keybyte = { };
        _keybyte = Encoding.UTF8.GetBytes(_key);
        SymmetricAlgorithm algorithm = DES.Create();
        var transform = algorithm.CreateDecryptor(_keybyte, privatekeyByte);
        var inputbuffer = Convert.FromBase64String(value);
        var outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
        return Encoding.Unicode.GetString(outputBuffer);
    }

    public async Task SendMail(string email, string message)
    {
        if (!string.IsNullOrEmpty(email) && email.Contains('@'))
        {
            var fromAddress = new MailAddress(_configSettings.MailSettings.Address, _configSettings.MailSettings.DisplayName);
            var toAddress = new MailAddress(email, email);

            var smtp = new SmtpClient
            {
                Host = _configSettings.MailSettings.Host,
                Port = int.Parse(_configSettings.MailSettings.Port),
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, _configSettings.MailSettings.MailKey)
            };

            using var data = new MailMessage(fromAddress, toAddress)
            {
                Subject = _configSettings.MailSettings.Subject,
                Body = message
            };

            await smtp.SendMailAsync(data);
        }
    }

    public string GetContentType()
    {
        return _configSettings.AuthSettings.ContentType;
    }
}