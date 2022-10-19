namespace Project.DTO.Role;

public record RoleToAddDto
{
    public int RoleId { get; set; }
    public string Name { get; set; }

    public string Key { get; set; }
}