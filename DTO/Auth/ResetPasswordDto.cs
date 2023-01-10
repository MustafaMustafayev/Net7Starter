using System.ComponentModel.DataAnnotations;

namespace DTO.Auth;

public record ResetPasswordDto
{
    public required string Email { get; set; }
    public string? VerificationCode { get; set; }

    [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",
        ErrorMessage = "Şifrə formatı düzgün deyil")]
    public required string Password { get; set; }

    [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",
        ErrorMessage = "Şifrə formatı düzgün deyil")]
    [Compare("Password")]
    public required string PasswordConfirmation { get; set; }
}