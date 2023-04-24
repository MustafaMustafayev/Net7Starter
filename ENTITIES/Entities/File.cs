using ENTITIES.Entities.Generic;

namespace ENTITIES.Entities;

public class File : Auditable, IEntity
{
    public required string OriginalName { get; set; }
    public required string HashName { get; set; }
    public required string Extension { get; set; }
    public required double Length { get; set; }
    public required string Path { get; set; }
}