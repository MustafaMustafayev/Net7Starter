namespace DTO.File;

public record FileToListDto(int FileId, string OriginalName, string HashName, string Extension, double Length,
    string? Path);

public record FileToAddDto(string OriginalName, string HashName, string Extension, double Length, string? Path);