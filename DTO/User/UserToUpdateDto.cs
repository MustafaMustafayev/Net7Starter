namespace DTO.User;

public record UserToUpdateDto
{
    public string Email { get; set; }
    public string ContactNumber { get; set; }
    public string Username { get; set; }
    public int? RoleId { get; set; }
}