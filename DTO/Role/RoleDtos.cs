using DTO.Permission;

namespace DTO.Role;

public record RoleToAddDto(string Name, string Key, List<int> PermissionIds);

public record RoleToListDto(int Id, string Name, string Key, List<PermissionToListDto> Permissions);
public record RoleToFkDto(int Id, string Name, string Key);

public record RoleToUpdateDto(string Name, string Key, List<int> PermissionIds);