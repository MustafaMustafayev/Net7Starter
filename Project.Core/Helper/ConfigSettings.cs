namespace Project.Core.Helper;

public record ConfigSettings
{
    public AuthSettings AuthSettings { get; set; }
    public ConnectionStrings ConnectionStrings { get; set; }
    public RequestSettings RequestSettings { get; set; }
    public SwaggerSettings SwaggerSettings { get; set; }
}