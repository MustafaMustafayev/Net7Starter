namespace DTO.Permission;

public record PermissionCreateRequestDto()
{
    public required string Name { get; set; }
    public required string Key { get; set; }
}