using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Entity.Entities;

public class User : AuditableEntity
{
    [Key] public int UserId { get; set; }

    [Required] public string Name { get; set; }

    [Required] public string Surname { get; set; }

    [Required] public string Fathername { get; set; }

    [Required] public string Gender { get; set; }

    [Required] public DateTime BirthDate { get; set; }

    public string ImagePath { get; set; }

    [Required] public string PIN { get; set; }

    [Required] public virtual Organization Organization { get; set; }

    [ForeignKey("Organization")] public int OrganizationId { get; set; }

    public string Position { get; set; }

    [Required] public virtual Role Role { get; set; }

    [ForeignKey("Role")] public int RoleId { get; set; }

    [Required][Phone] public string PhoneNumber { get; set; }

    [Required] public string OfficePhoneNumber { get; set; }

    [Required][EmailAddress] public string Email { get; set; }

    [Required] public bool IsActive { get; set; }

    public string Salt { get; set; }

    public string Password { get; set; }
}