using System.ComponentModel.DataAnnotations;

namespace DTO.Auth;

public record ResetPasswordDto()
{
    public string Email {  set; get; }
    public string? VerificationCode {  get; set; }
    [property: RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",
        ErrorMessage = "Şifrə formatı düzgün deyil")]
    public string Password { get; set; }
    [property: Compare("Password", ErrorMessage = "Şifrələr eyni deyil")]
    public string PasswordConfirmation { get; set; }
}