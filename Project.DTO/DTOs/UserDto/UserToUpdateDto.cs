using System.ComponentModel.DataAnnotations;

namespace Project.DTO.DTOs.UserDto;

public record UserToUpdateDto
{
    [Required] public int Id { get; set; }
    [Required] public string Email { get; set; }
    [Required] public string ContactNumber { get; set; }
    [Required] public string Username { get; set; }
}