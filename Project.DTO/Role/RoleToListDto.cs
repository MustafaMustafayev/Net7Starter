using System.ComponentModel.DataAnnotations;

namespace Project.DTO.Role;

public record RoleToListDto
{
    [Required] public int RoleId { get; set; }

    [Required] public string Name { get; set; }

    public string Key { get; set; }
}