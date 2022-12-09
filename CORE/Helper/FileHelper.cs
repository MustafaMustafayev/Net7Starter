using CORE.Localization;
using DTO.Responses;
using Microsoft.AspNetCore.Http;
using IResult = DTO.Responses.IResult;

namespace CORE.Helper;

public static class FileHelper
{
    public static async Task WriteFile(IFormFile file, string name, string path)
    {
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);

        await using var fileStream = new FileStream(Path.Combine(path, name), FileMode.Create);
        await file.CopyToAsync(fileStream);
    }

    public static async Task<IDataResult<string>> ReadFileAsByte64(string name, string path)
    {
        var filePath = Path.Combine(path, name);
        if (File.Exists(filePath))
            return new SuccessDataResult<string>(
                data: Convert.ToBase64String(await File.ReadAllBytesAsync(filePath)));

        return new ErrorDataResult<string>(Messages.FileIsNotFound.Translate());
    }

    public static IResult DeleteFile(string filePath)
    {
        if (!File.Exists(filePath)) return new SuccessResult();

        File.Delete(filePath);

        return new SuccessResult();
    }
}