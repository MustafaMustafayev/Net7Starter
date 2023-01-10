using System.ComponentModel.DataAnnotations;

namespace DTO.User;

public record UserToAddDto
{
    public required string Username { get; set; } = default!;
    public required string Email { get; set; } = default!;
    public required string ContactNumber { get; set; } = default!;

    [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",
        ErrorMessage = "Şifrə formatı düzgün deyil")]
    public string Password { get; set; } = default!;

    [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",
        ErrorMessage = "Şifrə formatı düzgün deyil")]
    public required string PasswordConfirmation { get; set; }

    public int? RoleId { get; set; }
}