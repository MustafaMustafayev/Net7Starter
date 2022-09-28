namespace Project.DTO.DTOs.RoleDTOs;

public record RoleToAddOrUpdateDto
{
    public string Name { get; set; }

    public string Key { get; set; }
}