using System.ComponentModel.DataAnnotations;

namespace DTO.User;

public record UserToAddDto
{
    [Required] public string Username { get; set; }
    [Required] public string Email { get; set; }
    [Required] public string ContactNumber { get; set; }

    [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",
        ErrorMessage = "Şifrə formatı düzgün deyil")]
    [Required]
    public string Password { get; set; }

    [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",
        ErrorMessage = "Şifrə formatı düzgün deyil")]
    [Compare("Password")]
    public string PasswordConfirmation { get; set; }
}