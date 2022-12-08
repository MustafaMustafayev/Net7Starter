using System.ComponentModel.DataAnnotations;

namespace ENTITIES.Entities;

public class Photo : Auditable, IEntity
{
    [Key] public int PhotoId { get; set; }

    public string FileName { get; set; }
    public string Extension { get; set; }
    public string Base64 { get; set; }
}