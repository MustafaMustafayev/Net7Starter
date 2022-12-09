using DTO.Role;

namespace DTO.User;

public record UserToListDto
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string ContactNumber { get; set; }
    public RoleToListDto Role { get; set; }
    public PhotoToListDto? Photo { get; set; }
}