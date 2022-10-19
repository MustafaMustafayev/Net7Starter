using System.ComponentModel.DataAnnotations;
using Project.Entity.Enums;

namespace Project.Entity.Entities;

public class User : AuditableEntity
{
    [Key] public int Id { get; set; }
    [Required] public string Username { get; set; }
    [Required] public string Email { get; set; }
    [Required] public string ContactNumber { get; set; }
    [Required] public string Password { get; set; }
    public string Salt { get; set; }
    [Required] public UserType Type { get; set; }

    [Display(Name = "İstifadəçiyə göndərilmiş sonuncu email təsdiqləmə kodu. Şifrəni yeniləyən zaman lazım olacaq.")]
    public string? LastVerificationCode { get; set; }
}