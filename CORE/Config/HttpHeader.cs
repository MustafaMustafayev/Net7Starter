namespace CORE.Config;

public record HttpHeader
{
    public string Name { get; set; }
    public dynamic Value { get; set; }
}