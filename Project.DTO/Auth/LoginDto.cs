using System.ComponentModel.DataAnnotations;

namespace Project.DTO.Auth;

public record LoginDto
{
    [Required] public string Email { get; set; }

    [Required] public string Password { get; set; }
}