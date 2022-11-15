namespace Project.Core.Config;

public record RedisSettings : Controllable
{
    public string Connection { get; set; }
}