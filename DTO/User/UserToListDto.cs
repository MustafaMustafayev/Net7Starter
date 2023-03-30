using DTO.Role;

namespace DTO.User;

public record UserToListDto
{
    public int UserId { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string ContactNumber { get; set; }
    public RoleToListDto? Role { get; set; }
}