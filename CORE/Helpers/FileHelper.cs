using Microsoft.AspNetCore.Http;

namespace CORE.Helpers;

public static class FileHelper
{
    public static async Task WriteFile(IFormFile file, string name, string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        await using var fileStream = new FileStream(Path.Combine(path, name), FileMode.Create);
        await file.CopyToAsync(fileStream);
    }

    public static async Task<string?> ReadFileAsByte64(string name, string path)
    {
        var filePath = Path.Combine(path, name);
        return File.Exists(filePath) ? Convert.ToBase64String(await File.ReadAllBytesAsync(filePath)) : null;
    }

    public static bool DeleteFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return false;
        }

        File.Delete(filePath);

        return true;
    }

    public static async Task<IFormFile?> ReadFileAsIFormFile(string name, string path)
    {
        var filePath = Path.Combine(path, name);
        if (!File.Exists(filePath))
        {
            return null; // or throw an exception based on your use case
        }

        var fileInfo = new FileInfo(filePath);
        var memoryStream = new MemoryStream();

        await using (var stream = fileInfo.OpenRead())
        {
            await stream.CopyToAsync(memoryStream);
        }

        memoryStream.Position = 0;

        return new FormFile(memoryStream, 0, memoryStream.Length, name, fileInfo.Name);
    }
}