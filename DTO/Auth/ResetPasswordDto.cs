using System.ComponentModel.DataAnnotations;

namespace DTO.Auth;

public record ResetPasswordDto
{
    public string Email { get; set; }
    public string VerificationCode { get; set; }

    [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",
        ErrorMessage = "Şifrə formatı düzgün deyil")]
    public string Password { get; set; }

    [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",
        ErrorMessage = "Şifrə formatı düzgün deyil")]
    [Compare("Password")]
    public string PasswordConfirmation { get; set; }
}