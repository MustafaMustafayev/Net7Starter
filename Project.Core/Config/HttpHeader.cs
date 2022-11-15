namespace Project.Core.Config;

public record HttpHeader
{
    public string Name { get; set; }
    public dynamic Value { get; set; }
}