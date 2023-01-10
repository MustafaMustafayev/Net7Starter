namespace DTO.User;

public record PhotoToListDto
{
    public int PhotoId { get; set; }
    public required string FileName { get; set; }
    public required string Extension { get; set; }
    public required string Base64 { get; set; }
}