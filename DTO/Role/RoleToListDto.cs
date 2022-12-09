using DTO.Permission;

namespace DTO.Role;

public record RoleToListDto
{
    public int RoleId { get; set; }

    public string Name { get; set; }

    public string Key { get; set; }
    public List<PermissionToListDto> Permissions { get; set; }
}