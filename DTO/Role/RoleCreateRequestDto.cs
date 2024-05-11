namespace DTO.Role;

public record RoleCreateRequestDto()
{
    public string Name { get; set; }
    public string Key { get; set; }
    public List<Guid> PermissionIds { get; set; }
}