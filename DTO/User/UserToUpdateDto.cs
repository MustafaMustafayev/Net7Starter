namespace DTO.User;

public record UserToUpdateDto
{
    public required string Email { get; set; }
    public required string ContactNumber { get; set; }
    public required string Username { get; set; }
    public int? RoleId { get; set; }
}