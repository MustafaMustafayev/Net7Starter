using System.ComponentModel.DataAnnotations;
using ENTITIES.Entities.Generic;

namespace ENTITIES.Entities;

public class Organization : Auditable, IEntity
{
    public int OrganizationId { get; set; }
    public required string FullName { get; set; }
    public required string ShortName { get; set; }
    public required string Address { get; set; }
    public virtual Organization? ParentOrganization { get; set; }
    public int? ParentOrganizationId { get; set; }
    [Phone] public required string PhoneNumber { get; set; }
    [StringLength(10)] public required string Tin { get; set; }
    [EmailAddress] public required string Email { get; set; }
    public required string Rekvizit { get; set; }
}