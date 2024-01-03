using DTO.Permission;

namespace DTO.Role;

public record RoleToAddDto(string Name, string Key, List<Guid> PermissionIds);

public record RoleToListDto(Guid Id, string Name, string Key, List<PermissionToListDto> Permissions);

public record RoleToFkDto(Guid Id, string Name, string Key);

public record RoleToUpdateDto(string Name, string Key, List<Guid> PermissionIds);