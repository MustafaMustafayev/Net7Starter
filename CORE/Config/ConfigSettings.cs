namespace CORE.Config;

public record ConfigSettings
{
    public AuthSettings AuthSettings { get; set; } = default!;
    public ConnectionStrings ConnectionStrings { get; set; } = default!;
    public RequestSettings RequestSettings { get; set; } = default!;
    public SwaggerSettings SwaggerSettings { get; set; } = default!;
    public RedisSettings RedisSettings { get; set; } = default!;
    public ElasticSearchSettings ElasticSearchSettings { get; set; } = default!;
    public SentrySettings SentrySettings { get; set; } = default!;
    public HttpClientSettings FirstHttpClientSettings { get; set; } = default!;
    public CryptographySettings CryptographySettings { get; set; } = default!;
    public MailSettings MailSettings { get; set; } = default!;
    public SftpSettings SftpSettings { get; set; } = default!;
}