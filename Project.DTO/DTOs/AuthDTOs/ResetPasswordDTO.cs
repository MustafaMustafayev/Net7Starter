using System.ComponentModel.DataAnnotations;

namespace Project.DTO.DTOs.AuthDTOs;

public record ResetPasswordDTO
{
    public int UserId { get; set; }
    public int Age { get; set; }
    [DataType(DataType.Password)] public string Password { get; set; }

    [DataType(DataType.Password)] public string PasswordConfirmation { get; set; }
}