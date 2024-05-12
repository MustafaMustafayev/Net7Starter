namespace DTO.Permission;

public record PermissionUpdateRequestDto()
{
    public required string Name { get; set; }
    public required string Key { get; set; }
}