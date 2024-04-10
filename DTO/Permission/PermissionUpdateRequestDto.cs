namespace DTO.Permission;

public record PermissionUpdateRequestDto()
{
    public string Name { get; set; }
    public string Key { get; set; }
}