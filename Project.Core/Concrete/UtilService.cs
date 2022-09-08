using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Project.Core.Abstract;
using Project.Core.Enums;
using Project.Core.Helper;

namespace Project.Core.Concrete;

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

    public int? GetUserIdFromToken(string tokenString)
    {
        if (string.IsNullOrEmpty(tokenString)) return null;

        var jwtEncodedString = tokenString[7..];
        var token = new JwtSecurityToken(jwtEncodedString);
        var userId =
            Convert.ToInt32(token.Claims.First(c => c.Type == _configSettings.AuthSettings.TokeNameIdKey).Value);
        return userId;
    }

    public bool IsValidToken(string tokenString)
    {
        if (string.IsNullOrEmpty(tokenString) || tokenString.Length < 7) return false;

        tokenString = tokenString[7..];
        var tokenHandler = new JwtSecurityTokenHandler();
        var secretKey = Encoding.ASCII.GetBytes(_configSettings.AuthSettings.SecretKey);
        try
        {
#pragma warning disable SA1117 // Parameters should be on same line or separate lines
            tokenHandler.ValidateToken(tokenString, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = false,
                IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);
#pragma warning restore SA1117 // Parameters should be on same line or separate lines

            var jwtToken = (JwtSecurityToken)validatedToken;
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public IEnumerable<string> GetFilterKeys()
    {
        return EnumConverter<EBookSearchFilterKey>.GetAllValuesAsIEnumerable();
    }

    public string GetContentType()
    {
        return _configSettings.AuthSettings.ContentType;
    }
}