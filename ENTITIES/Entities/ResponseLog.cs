using ENTITIES.Entities.Generic;

namespace ENTITIES.Entities;

public class ResponseLog : IEntity
{
    public Guid Id { get; set; }

    public string? TraceIdentifier { get; set; }

    public DateTimeOffset ResponseDate { get; set; }

    public string? StatusCode { get; set; }

    public string? Token { get; set; }

    public Guid? UserId { get; set; }

    public bool IsDeleted { get; set; }
}