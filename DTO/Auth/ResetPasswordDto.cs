using System.ComponentModel.DataAnnotations;

namespace DTO.Auth;

public record ResetPasswordDto
{
    [Required] public string Email { get; set; }
    [Required] public string VerificationCode { get; set; }

    [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",
        ErrorMessage = "Şifrə formatı düzgün deyil")]
    [Required]
    public string Password { get; set; }

    [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",
        ErrorMessage = "Şifrə formatı düzgün deyil")]
    [Required]
    [Compare("Password")]
    public string PasswordConfirmation { get; set; }
}