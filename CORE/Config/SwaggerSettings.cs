namespace CORE.Config;

public record SwaggerSettings : Controllable
{
    public string Title { get; set; }
    public string Version { get; set; }
    public string Theme { get; set; }
}