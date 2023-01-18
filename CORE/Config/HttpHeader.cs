namespace CORE.Config;

public record HttpHeader
{
    public required string Name { get; set; }
    public required dynamic Value { get; set; }
}