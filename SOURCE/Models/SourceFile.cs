namespace SOURCE.Models;

public class SourceFile
{
    public required string Path { get; set; }
    public required string Name { get; set; }
    public required string Text { get; set; }
    public required bool SkipIfExists { get; set; } = true;
}