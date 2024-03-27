namespace DTO.Permission;

public record PermissionToListDto()
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Key { get; set; }
}
