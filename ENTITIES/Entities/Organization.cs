using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENTITIES.Entities;

public class Organization : Auditable, IEntity
{
    [Key] public int OrganizationId { get; set; }

    [Required] public string FullName { get; set; }

    [Required] public string ShortName { get; set; }

    [Required] public string Address { get; set; }

    public virtual Organization ParentOrganization { get; set; }

    [ForeignKey("ParentOrganization")] public int? ParentOrganizationId { get; set; }

    [Required] [Phone] public string PhoneNumber { get; set; }

    [Required] [StringLength(10)] public string Tin { get; set; }

    [Required] [EmailAddress] public string Email { get; set; }

    [Required] public string Rekvizit { get; set; }
}