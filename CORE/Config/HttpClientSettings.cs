namespace CORE.Config;

public record HttpClientSettings
{
    public required string TokenPrefix { get; set; }
    public required string BaseUrl { get; set; }
    public required string Name { get; set; }
    public required int TimeoutInSeconds { get; set; }
    public required List<HttpHeader> Headers { get; set; }
}