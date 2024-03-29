using DTO.Permission;

namespace DTO.Role;

public record RoleToListDto()
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Key { get; set; }
    public List<PermissionToListDto> Permissions { get; set; }
}
