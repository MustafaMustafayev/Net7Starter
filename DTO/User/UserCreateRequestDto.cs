using System.ComponentModel.DataAnnotations;

namespace DTO.User;

public record UserCreateRequestDto()
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string ContactNumber { get; set; }
    [property: RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",
        ErrorMessage = "Şifrə formatı düzgün deyil")]
    public required string Password { get; set; }
    [property: Compare("Password", ErrorMessage = "Şifrələr eyni deyil")]
    public required string PasswordConfirmation { get; set; }
    public Guid? RoleId { get; set; }
}