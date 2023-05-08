using System.ComponentModel.DataAnnotations;
using DTO.Role;

namespace DTO.User;

public record UserToAddDto
(
    string Username,
    string Email,
    string ContactNumber,
    [property: RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",
        ErrorMessage = "Şifrə formatı düzgün deyil")]
    string Password,
    [property: Compare("Password", ErrorMessage = "Şifrələr eyni deyil")]
    string PasswordConfirmation,
    int? RoleId
);

public record UserToListDto
(
    int UserId,
    string Username,
    string Email,
    string ContactNumber,
    RoleToListDto? Role,
    string? ProfileFileHashName
);

public record UserToUpdateDto
(
    string Email,
    string ContactNumber,
    string Username,
    int? RoleId
);