namespace DTO.User;

public record PhotoToListDto
{
    public int PhotoId { get; set; }
    public string FileName { get; set; }
    public string Extension { get; set; }
    public string Base64 { get; set; }
}