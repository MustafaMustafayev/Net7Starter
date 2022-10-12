using System.ComponentModel.DataAnnotations;

namespace Project.DTO.Auth;

public record ResetPasswordDto
{
    [Required] public string Email { get; set; }
    [Required] public string VerificationCode { get; set; }
    [Required] public string Password { get; set; }
    [Required] [Compare("Password")] public string PasswordConfirmation { get; set; }
}