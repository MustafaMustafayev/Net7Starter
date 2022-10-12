namespace Project.DTO.Role;

public record RoleToAddOrUpdateDto
{
    public string Name { get; set; }

    public string Key { get; set; }
}