using System.ComponentModel.DataAnnotations;
using ENTITIES.Entities.Generic;

namespace ENTITIES.Entities;

public class ResponseLog : IEntity
{
    [Key] public int ResponseLogId { get; set; }

    public string? TraceIdentifier { get; set; }

    public DateTimeOffset ResponseDate { get; set; }

    public string? StatusCode { get; set; }

    public string? Token { get; set; }

    public int? UserId { get; set; }

    public bool IsDeleted { get; set; }
}