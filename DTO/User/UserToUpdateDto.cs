using System.ComponentModel.DataAnnotations;

namespace DTO.User;

public record UserToUpdateDto
{
    [Required] public string Email { get; set; }
    [Required] public string ContactNumber { get; set; }
    [Required] public string Username { get; set; }
}