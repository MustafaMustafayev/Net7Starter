using System.ComponentModel.DataAnnotations;

namespace ENTITIES.Entities;

public class User : Auditable
{
    [Key] public int UserId { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string ContactNumber { get; set; }
    public required string Password { get; set; }
    public required string Salt { get; set; }

    [Display(Name = "İstifadəçiyə göndərilmiş sonuncu email təsdiqləmə kodu. Şifrəni yeniləyən zaman lazım olacaq.")]
    public string? LastVerificationCode { get; set; }
    public int? RoleId { get; set; }
    public virtual Role? Role { get; set; }
    public string? ImagePath { get; set; }
}