using System.ComponentModel.DataAnnotations;
using DTO.User;

namespace DTO.Auth;

public record LoginDto(string Email, string Password);

public record LoginResponseDto
(
    UserToListDto User,
    string AccessToken,
    DateTime AccessTokenExpireDate,
    string RefreshToken,
    DateTime RefreshTokenExpireDate
);

public record ResetPasswordDto
(
    string Email,
    string? VerificationCode,
    [property: RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",
        ErrorMessage = "Şifrə formatı düzgün deyil")]
    string Password,
    [property: Compare("Password", ErrorMessage = "Şifrələr eyni deyil")]
    string PasswordConfirmation
);