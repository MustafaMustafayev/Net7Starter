using DTO.Permission;

namespace DTO.Role;

public record RoleByIdResponseDto()
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Key { get; set; }
    public List<PermissionResponseDto>? Permissions { get; set; }
}