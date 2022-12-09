using System.ComponentModel.DataAnnotations;

namespace ENTITIES.Entities;

public class Role : Auditable, IEntity
{
    [Key] public int RoleId { get; set; }

    [Required] public string Name { get; set; }
    public string Key { get; set; }
    public virtual List<Permission> Permissions { get; set; }
}