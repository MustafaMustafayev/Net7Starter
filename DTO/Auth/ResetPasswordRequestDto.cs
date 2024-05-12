using System.ComponentModel.DataAnnotations;

namespace DTO.Auth;

public record ResetPasswordRequestDto()
{
    public required string Email { set; get; }
    public string? VerificationCode { get; set; }
    [property: RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",
        ErrorMessage = "Şifrə formatı düzgün deyil")]
    public required string Password { get; set; }
    [property: Compare("Password", ErrorMessage = "Şifrələr eyni deyil")]
    public required string PasswordConfirmation { get; set; }
}