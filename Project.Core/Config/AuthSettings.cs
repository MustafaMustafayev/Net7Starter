namespace Project.Core.Config;

public record AuthSettings
{
    public string HeaderName { get; set; }
    public string TokenPrefix { get; set; }
    public string ContentType { get; set; }
    public string SecretKey { get; set; }
    public string TokenUserIdKey { get; set; }
    public string TokenCompanyIdKey { get; set; }
    public int TokenExpirationTimeInHours { get; set; }
}