namespace CORE.Config;

public record AuthSettings
{
    public required string Type { get; set; }
    public required string HeaderName { get; set; }
    public required string RefreshTokenHeaderName { get; set; }
    public required string TokenPrefix { get; set; }
    public required string ContentType { get; set; }
    public required string SecretKey { get; set; }
    public required string TokenUserIdKey { get; set; }
    public required string TokenCompanyIdKey { get; set; }
    public required int TokenExpirationTimeInHours { get; set; }
    public required int RefreshTokenAdditionalMinutes { get; set; }
}