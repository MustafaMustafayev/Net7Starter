using System.ComponentModel.DataAnnotations;

namespace ENTITIES.Entities;

public class User : Auditable
{
    [Key] public int UserId { get; set; }
    [Required] public string Username { get; set; }
    [Required] public string Email { get; set; }
    [Required] public string ContactNumber { get; set; }
    [Required] public string Password { get; set; }
    public string Salt { get; set; }

    [Display(Name = "İstifadəçiyə göndərilmiş sonuncu email təsdiqləmə kodu. Şifrəni yeniləyən zaman lazım olacaq.")]
    public string? LastVerificationCode { get; set; }

    [Required] public int RoleId { get; set; }
    [Required] public virtual Role Role { get; set; }
    public string? ImagePath { get; set; }
}