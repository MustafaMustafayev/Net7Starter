namespace CORE.Config;

public record AuthSettings
{
    public string Type { get; set; }
    public string HeaderName { get; set; }
    public string RefreshTokenHeaderName { get; set; }
    public string TokenPrefix { get; set; }
    public string ContentType { get; set; }
    public string SecretKey { get; set; }
    public string TokenUserIdKey { get; set; }
    public string TokenCompanyIdKey { get; set; }
    public int TokenExpirationTimeInHours { get; set; }
    public int RefreshTokenAdditionalMinutes { get; set; }
}