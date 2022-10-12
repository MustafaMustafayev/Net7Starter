namespace Project.DTO.DTOs.RoleDto;

public record RoleToAddOrUpdateDto
{
    public string Name { get; set; }

    public string Key { get; set; }
}