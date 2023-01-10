namespace DTO.Permission;

public record PermissionToAddDto
{
    public required string Name { get; set; }
    public required string Key { get; set; }
}