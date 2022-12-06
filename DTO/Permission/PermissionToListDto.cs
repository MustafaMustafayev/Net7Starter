namespace DTO.Permission;

public record PermissionToListDto
{
    public int PermissionId { get; set; }
    public string Name { get; set; }
    public string Key { get; set; }
}