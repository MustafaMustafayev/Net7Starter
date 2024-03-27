using ENTITIES.Entities.Generic;

namespace ENTITIES.Entities
{
    public class Test : Auditable, IEntity
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
