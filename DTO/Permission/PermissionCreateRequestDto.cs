namespace DTO.Permission;

public record PermissionCreateRequestDto()
{
    public string Name { get; set; }
    public string Key { get; set; }
}
