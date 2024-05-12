using ENTITIES.Entities.Generic;

namespace ENTITIES.Entities;

public class Nlog : IEntity
{
    public Guid Id { get; set; }

    public string? Application { get; set; }

    public DateTime LogDate { get; set; }

    public string? Level { get; set; }

    public string? Message { get; set; }

    public string? CallSite { get; set; }

    public string? Logger { get; set; }

    public string? Exception { get; set; }
}