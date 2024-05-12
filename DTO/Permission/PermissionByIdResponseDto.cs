namespace DTO.Permission;

public record PermissionByIdResponseDto()
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Key { get; set; }
}