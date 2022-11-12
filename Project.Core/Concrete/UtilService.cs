using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Project.Core.Abstract;
using Project.Core.Config;

namespace Project.Core.Concrete;

public class UtilService : IUtilService
{
    private readonly ConfigSettings _configSettings;
    private readonly IMemoryCache _memoryCache;

    public UtilService(ConfigSettings configSettings, IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
        _configSettings = configSettings;
    }

    public HttpContent GetHttpContentObject(object obj)
    {
        return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, GetContentType());
    }

    public int? GetUserIdFromToken(string? tokenString)
    {
        if (string.IsNullOrEmpty(tokenString)) return null;
        if (!tokenString.Contains("Bearer ")) return null;

        var token = new JwtSecurityToken(tokenString[7..]);

        return Convert.ToInt32(token.Claims.First(c => c.Type == _configSettings.AuthSettings.TokenUserIdKey).Value);
    }

    public int? GetCompanyIdFromToken(string? tokenString)
    {
        if (string.IsNullOrEmpty(tokenString)) return null;
        if (!tokenString.Contains("Bearer ")) return null;

        var token = new JwtSecurityToken(tokenString[7..]);
        var companyIdClaim = token.Claims.First(c => c.Type == _configSettings.AuthSettings.TokenCompanyIdKey);

        if (companyIdClaim is null || string.IsNullOrEmpty(companyIdClaim.Value)) return null;

        return Convert.ToInt32(companyIdClaim.Value);
    }

    public bool IsValidToken(string tokenString)
    {
        if (string.IsNullOrEmpty(tokenString) || tokenString.Length < 7) return false;

        tokenString = tokenString[7..];
        var tokenHandler = new JwtSecurityTokenHandler();
        var secretKey = Encoding.ASCII.GetBytes(_configSettings.AuthSettings.SecretKey);
        try
        {
            tokenHandler.ValidateToken(tokenString, new TokenValidationParameters
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

    public void AddTokenToCache(string token, DateTime expireDate)
    {
        Dictionary<string, DateTime>? tokens;

        _memoryCache.TryGetValue(Constants.Constants.CacheTokensKey, out tokens);

        if (tokens is null) tokens = new Dictionary<string, DateTime>();

        tokens.Add($"{_configSettings.AuthSettings.TokenPrefix} {token}", expireDate);

        _memoryCache.Set(Constants.Constants.CacheTokensKey, tokens,
            TimeSpan.FromHours(_configSettings.AuthSettings.TokenExpirationTimeInHours));
    }

    public bool IsTokenExistsInCache(string? token)
    {
        if (string.IsNullOrEmpty(token)) return false;
        Dictionary<string, DateTime>? tokens;

        _memoryCache.TryGetValue(Constants.Constants.CacheTokensKey, out tokens);

        if (tokens is null || !tokens.Any()) return false;

        return tokens.ContainsKey(token);
    }

    public string GetContentType()
    {
        return _configSettings.AuthSettings.ContentType;
    }
}