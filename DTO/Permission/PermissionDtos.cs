namespace DTO.Permission;

public record PermissionToAddDto(string Name, string Key);

public record PermissionToListDto(int PermissionId, string Name, string Key);

public record PermissionToUpdateDto(string Name, string Key);