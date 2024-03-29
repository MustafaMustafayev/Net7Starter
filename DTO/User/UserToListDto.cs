using DTO.Role;

namespace DTO.User;

public record UserToListDto()
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string ContactNumber { get; set; }
    public RoleToFkDto? Role { get; set; }
    public string? File { get; set; }
}
