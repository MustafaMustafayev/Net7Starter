using System.ComponentModel.DataAnnotations;

namespace DTO.User;

public record UserCreateRequestDto()
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string ContactNumber { get; set; }
    [property: RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",
        ErrorMessage = "Şifrə formatı düzgün deyil")]
    public string Password { get; set; }
    [property: Compare("Password", ErrorMessage = "Şifrələr eyni deyil")]
    public string PasswordConfirmation { get; set; }
    public Guid? RoleId { get; set; }
}
