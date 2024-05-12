namespace DTO.Role;

public record RoleUpdateRequestDto()
{
    public required string Name { get; set; }
    public required string Key { get; set; }
    public List<Guid>? PermissionIds { get; set; }
}