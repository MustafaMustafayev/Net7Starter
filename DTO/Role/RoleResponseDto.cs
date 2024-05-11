using DTO.Permission;

namespace DTO.Role;

public record RoleResponseDto()
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Key { get; set; }
    public List<PermissionResponseDto> Permissions { get; set; }
}