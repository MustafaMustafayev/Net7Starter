using System.ComponentModel.DataAnnotations;
using ENTITIES.Enums;

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

    [Required] public UserType Type { get; set; }
    public virtual Photo? Photo { get; set; }
}