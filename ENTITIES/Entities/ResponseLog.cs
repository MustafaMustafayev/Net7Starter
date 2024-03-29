﻿using ENTITIES.Entities.Generic;
using System.ComponentModel.DataAnnotations;

namespace ENTITIES.Entities;

public class ResponseLog : IEntity
{
    [Key] public Guid ResponseLogId { get; set; }

    public string? TraceIdentifier { get; set; }

    public DateTimeOffset ResponseDate { get; set; }

    public string? StatusCode { get; set; }

    public string? Token { get; set; }

    public Guid? UserId { get; set; }

    public bool IsDeleted { get; set; }
}