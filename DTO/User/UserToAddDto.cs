using System.ComponentModel.DataAnnotations;

namespace DTO.User;

public record UserToAddDto
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string ContactNumber { get; set; }

    [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",
        ErrorMessage = "Şifrə formatı düzgün deyil")]
    public string Password { get; set; }

    [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",
        ErrorMessage = "Şifrə formatı düzgün deyil")]
    public string PasswordConfirmation { get; set; }

    public int? RoleId { get; set; }
}