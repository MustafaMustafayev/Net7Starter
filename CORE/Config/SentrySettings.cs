namespace CORE.Config;

public record SentrySettings : Controllable
{
    public bool IsEnabled { get; set; }
}