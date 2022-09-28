using System.ComponentModel.DataAnnotations;

namespace Project.DTO.DTOs.UserDto;

public record UserToAddDto
{
    [Required] public string Username { get; set; }
    [Required] public string Email { get; set; }
    [Required] public string ContactNumber { get; set; }

    [Required] public string Password { get; set; }

    [Compare("Password")] public string PasswordConfirmation { get; set; }
}