using System.IdentityModel.Tokens.Jwt;
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
        string userId = Decrypt(token.Claims.First(c => c.Type == _configSettings.AuthSettings.TokenUserIdKey).Value);
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

    public string GetContentType()
    {
        return _configSettings.AuthSettings.ContentType;
    }

    public string Encrypt(string value)
    {
        byte[] key = Convert.FromBase64String(_configSettings.CryptographySettings.KeyBase64);
        byte[] iv = Convert.FromBase64String(_configSettings.CryptographySettings.VBase64);
        SymmetricAlgorithm algorithm = DES.Create();
        ICryptoTransform transform = algorithm.CreateEncryptor(key, iv);
        byte[] inputbuffer = Encoding.Unicode.GetBytes(value);
        byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
        return Convert.ToBase64String(outputBuffer);  
    }

    public string Decrypt(string value)
    {
        byte[] key = Convert.FromBase64String(_configSettings.CryptographySettings.KeyBase64);
        byte[] iv = Convert.FromBase64String(_configSettings.CryptographySettings.VBase64);
        SymmetricAlgorithm algorithm = DES.Create();
        ICryptoTransform transform = algorithm.CreateDecryptor(key, iv);
        byte[] inputbuffer = Convert.FromBase64String(value);
        byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
        return Encoding.Unicode.GetString(outputBuffer);
    }
  }