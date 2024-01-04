using DTO.Role;
using System.ComponentModel.DataAnnotations;

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
    Guid? RoleId
);

public record UserToListDto
(
    Guid Id,
    string Username,
    string Email,
    string ContactNumber,
    RoleToFkDto? Role,
    string? File
);

public record UserToUpdateDto
(
    string Email,
    string ContactNumber,
    string Username,
    Guid? RoleId
);