using System.ComponentModel.DataAnnotations;

namespace Project.DTO.DTOs.AuthDto;

public record LoginDto
{
    [Required] public string Email { get; set; }

    [Required] public string Password { get; set; }
}