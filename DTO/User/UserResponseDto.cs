using DTO.Role;

namespace DTO.User;

public record UserResponseDto()
{
    public required Guid Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string ContactNumber { get; set; }
    public RoleToFkDto? Role { get; set; }
    public string? Image { get; set; }
}