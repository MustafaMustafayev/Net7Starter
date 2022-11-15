namespace Project.DTO.User;

public record UserPhotoDto
{
    public string? ProfilePhotoFileName { get; set; }
    public string? ProfilePhotoExtension { get; set; }
    public string? ProfilePhotoBase64 { get; set; }
}