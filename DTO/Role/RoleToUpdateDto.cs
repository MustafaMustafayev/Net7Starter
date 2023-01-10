namespace DTO.Role;

public record RoleToUpdateDto
{
    public string Name { get; set; }

    public required string Key { get; set; }
    public List<int>? PermissionIds { get; set; }
}