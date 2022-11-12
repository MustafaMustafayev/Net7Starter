namespace Project.Core.Config;

public record HttpClientSettings
{
    public string TokenPrefix { get; set; }
    public string BaseUrl { get; set; }
    public string Name { get; set; }
    public int TimeoutInSeconds { get; set; }
    public List<HttpHeader> Headers { get; set; }
}