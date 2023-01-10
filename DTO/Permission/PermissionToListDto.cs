namespace DTO.Permission;

public record PermissionToListDto
{
    public int PermissionId { get; set; }
    public required string Name { get; set; }
    public required string Key { get; set; }
}