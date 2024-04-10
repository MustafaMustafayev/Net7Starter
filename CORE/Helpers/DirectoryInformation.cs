public record DirectoryInformation
{
    public required string Name { get; set; }
    public required string Length { get; set; }
    public required string Path { get; set; }
    public required string CreatedAt { get; set; }
    public required bool IsDirectory { get; set; }
}