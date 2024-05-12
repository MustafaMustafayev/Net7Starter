using CORE.Enums;

namespace DTO.File;

public record FileCreateRequestDto
{
    public required string OriginalName { get; set; }
    public required string HashName { get; set; }
    public required string Extension { get; set; }
    public required double Length { get; set; }
    public required string Path { get; set; }
    public required EFileType Type { get; set; }
}