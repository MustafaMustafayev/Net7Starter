namespace Project.Core.Helper;

public record AuthSettings
{
    public string HeaderName { get; set; }
    public string TokenPrefix { get; set; }
    public string ContentType { get; set; }
    public string SecretKey { get; set; }
    public string TokeNameIdKey { get; set; }
    public string TokenCompanyIdKey { get; set; }
}