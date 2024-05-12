using ENTITIES.Entities.Generic;

namespace ENTITIES.Entities.Test;
public class Test : IEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
}