using Project.Entity.Enums;

namespace Project.DTO.DTOs.UserDto;

public record UserToListDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string ContactNumber { get; set; }
    public UserType Type { get; set; }
}