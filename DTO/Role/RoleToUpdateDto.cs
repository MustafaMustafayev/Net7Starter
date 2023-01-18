namespace DTO.Role;

public record RoleToUpdateDto
{
    public required string Name { get; set; }

    public required string Key { get; set; }
    public List<int>? PermissionIds { get; set; }
}