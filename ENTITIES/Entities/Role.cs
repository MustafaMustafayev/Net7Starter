using System.ComponentModel.DataAnnotations;

namespace ENTITIES.Entities;

public class Role : AuditableEntity, IEntity
{
    [Key] public int RoleId { get; set; }

    [Required] public string Name { get; set; }

    public string Key { get; set; }
}