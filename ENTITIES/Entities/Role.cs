using ENTITIES.Entities.Generic;

namespace ENTITIES.Entities;

public class Role : Auditable, IEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Key { get; set; }
    public virtual List<Permission>? Permissions { get; set; }
}