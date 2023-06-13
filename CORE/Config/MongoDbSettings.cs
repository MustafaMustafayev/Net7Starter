namespace CORE.Config;

public record MongoDbSettings : Controllable
{
    public required string Connection { get; set; }
    public required string DefaultDatabase { get; set; }
}