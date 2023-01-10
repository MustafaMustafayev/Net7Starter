namespace DTO.Permission;

public record PermissionToUpdateDto
{
    public required string Name { get; set; }
    public required string Key { get; set; }
}