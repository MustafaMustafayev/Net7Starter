using DTO.Role;

namespace DTO.User;

public record UserResponseDto()
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string ContactNumber { get; set; }
    public RoleToFkDto? Role { get; set; }
    public string? File { get; set; }
}
