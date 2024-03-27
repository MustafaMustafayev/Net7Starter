namespace DTO.Permission;

public record PermissionToAddDto() {
    public string Name { get; set; }
    public string Key { get; set; }
}
