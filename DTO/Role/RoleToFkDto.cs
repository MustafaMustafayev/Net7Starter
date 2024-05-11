namespace DTO.Role;

public record RoleToFkDto()
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Key { get; set; }
};