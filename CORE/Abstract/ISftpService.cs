using Microsoft.AspNetCore.Http;

namespace CORE.Abstract;

public interface ISftpService
{
    List<DirectoryInformation> GetDirectoryInformation(string path);
    void UploadFile(string filePath, string fileName, IFormFile formFile);
    void DeleteFile(string filePath);
    byte[] ReadImage(string filePath);
    byte[] CompressImage(int jpegQuality, byte[] data);
}