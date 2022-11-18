namespace CORE.Config;

public record ConfigSettings
{
    public AuthSettings AuthSettings { get; set; }
    public ConnectionStrings ConnectionStrings { get; set; }
    public RequestSettings RequestSettings { get; set; }
    public SwaggerSettings SwaggerSettings { get; set; }
    public RedisSettings RedisSettings { get; set; }
    public SentrySettings SentrySettings { get; set; }
    public HttpClientSettings FirstHttpClientSettings { get; set; }
}