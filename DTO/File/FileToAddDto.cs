using ENTITIES.Enums;

namespace DTO.File;

public record FileToAddDto
{
    public string OriginalName { get; set; }
    public string HashName { get; set; }
    public string Extension { get; set; }
    public double Length { get; set; }
    public string Path { get; set; }
    public FileType Type { get; set; }
}
