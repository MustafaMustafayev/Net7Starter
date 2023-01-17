namespace SourceBuilder.Models;

public class SourceFile
{
    public required string Path { get; set; }
    public required string Name { get; set; }
    public required string Text { get; set; }
}