using DTO.Permission;

namespace DTO.Role;

public record RoleResponseDto()
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Key { get; set; }
    public List<PermissionResponseDto>? Permissions { get; set; }
}