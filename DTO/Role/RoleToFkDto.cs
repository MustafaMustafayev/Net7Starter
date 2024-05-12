namespace DTO.Role;

public record RoleToFkDto()
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Key { get; set; }
};