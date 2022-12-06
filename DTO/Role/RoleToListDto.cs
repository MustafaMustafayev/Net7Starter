using System.ComponentModel.DataAnnotations;
using DTO.Permission;

namespace DTO.Role;

public record RoleToListDto
{
    [Required] public int RoleId { get; set; }

    [Required] public string Name { get; set; }

    public string Key { get; set; }
    public List<PermissionToListDto> Permissions { get; set; }
}