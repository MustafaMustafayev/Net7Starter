namespace DTO.User;

public record UserUpdateRequestDto()
{
    public required string Email { get; set; }
    public required string ContactNumber { get; set; }
    public required string Username { get; set; }
    public Guid? RoleId { get; set; }
}