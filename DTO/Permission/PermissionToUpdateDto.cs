namespace DTO.Permission;

public record PermissionToUpdateDto
{
    public string Name { get; set; }
    public string Key { get; set; }
}