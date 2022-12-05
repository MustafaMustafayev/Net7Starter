using System.ComponentModel.DataAnnotations;

namespace DTO.User;

public class Photo
{
    [Key] public int PhotoId { get; set; }

    public string FileName { get; set; }
    public string Extension { get; set; }
    public string Base64 { get; set; }
}