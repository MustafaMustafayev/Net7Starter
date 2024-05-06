﻿using ENTITIES.Entities.Generic;
using System.ComponentModel.DataAnnotations;

namespace ENTITIES.Entities;

public class Organization : Auditable, IEntity
{
    public Guid Id { get; set; }
    public required string FullName { get; set; }
    public required string ShortName { get; set; }
    public required string Address { get; set; }
    public virtual Organization? Parent { get; set; }
    public Guid? ParentId { get; set; }
    [Phone] public required string PhoneNumber { get; set; }
    [StringLength(10)] public required string Tin { get; set; }
    [EmailAddress] public required string Email { get; set; }
    public required string Rekvizit { get; set; }
    public Guid? LogoFileId { get; set; }
    public virtual File? LogoFile { get; set; }
}