namespace CORE.Config;

public record RedisSettings : Controllable
{
    public required string Connection { get; set; }
}