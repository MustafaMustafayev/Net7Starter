using DTO.Permission;

namespace DTO.Role;

public record RoleToAddDto(string Name, string Key, List<int> PermissionIds);

public record RoleToListDto(int RoleId, string Name, string Key, List<PermissionToListDto> Permissions);

public record RoleToUpdateDto(string Name, string Key, List<int> PermissionIds);