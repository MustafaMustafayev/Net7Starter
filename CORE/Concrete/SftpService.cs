using CORE.Abstract;
using CORE.Config;
using DTO.Sftp;
using Microsoft.AspNetCore.Http;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using System.Drawing;
using System.Drawing.Imaging;
using ConnectionInfo = Renci.SshNet.ConnectionInfo;

namespace CORE.Concrete;

public class SftpService : ISftpService
{
    private readonly ConfigSettings _configSettings;

    public SftpService(ConfigSettings configSettings)
    {
        _configSettings = configSettings;
    }

    public List<DirectoryInformation> GetDirectoryInformation(string path)
    {
        if (string.IsNullOrEmpty(path) || string.IsNullOrWhiteSpace(path)) path = string.Empty;

        var directoryInfos = new List<DirectoryInformation>();
        var connectionInfo = GetConnectionInfo();
        using (var sftp = new SftpClient(connectionInfo))
        {
            sftp.Connect();
            if (!sftp.IsConnected) return directoryInfos.OrderBy(m => m.Name).ThenBy(m => !m.IsDirectory).ToList();

            var realPath = ("/" + path).Replace("//", "/");
            sftp.ChangeDirectory(realPath);
            ICollection<ISftpFile> directories = sftp.ListDirectory(realPath).ToList();
            directoryInfos.AddRange(from sftpFile in directories
                                    let fileName = sftpFile.Name
                                    let fileParts = fileName.Split('.')
                                    where fileName != "." && fileName != ".."
                                    let isAvaliable = sftpFile.IsDirectory || fileParts[^1] == "mp4"
                                    where isAvaliable
                                    let subDirectory = sftpFile.FullName.StartsWith("/") && sftpFile.FullName.Length > 0
                                        ? sftpFile.FullName.Remove(0, 1)
                                        : sftpFile.FullName
                                    select new DirectoryInformation
                                    {
                                        Name = sftpFile.Name,
                                        Length = sftpFile.Length.ToString(),
                                        Path = subDirectory,
                                        CreatedAt = sftpFile.LastWriteTime.ToString("dd-MMM-yyyy HH:mm"),
                                        IsDirectory = sftpFile.IsDirectory
                                    });
        }

        return directoryInfos.OrderBy(m => m.Name).ThenBy(m => !m.IsDirectory).ToList();
    }

    public void UploadFile(string filePath, string fileName, IFormFile formFile)
    {
        var connectionInfo = GetConnectionInfo();
        using var sftp = new SftpClient(connectionInfo);
        sftp.Connect();
        if (!sftp.IsConnected) return;

        sftp.ChangeDirectory(filePath);
        using (var ms = new MemoryStream())
        {
            formFile.CopyTo(ms);
            var fileBytes = ms.ToArray();
            sftp.WriteAllBytes(filePath + "/" + fileName, fileBytes);
        }

        sftp.Disconnect();
    }

    public void DeleteFile(string filePath)
    {
        var connectionInfo = GetConnectionInfo();
        using var sftp = new SftpClient(connectionInfo);
        sftp.Connect();
        if (!sftp.IsConnected) return;

        sftp.ChangeDirectory(filePath);

        if (sftp.Exists(filePath)) sftp.Delete(filePath);
    }

    public byte[] ReadImage(string filePath)
    {
        var imageBytes = Array.Empty<byte>();

        var connectionInfo = GetConnectionInfo();
        using var sftp = new SftpClient(connectionInfo);
        sftp.Connect();
        if (!sftp.IsConnected) return imageBytes;

        sftp.ChangeDirectory(filePath);

        if (string.IsNullOrEmpty(filePath)) return imageBytes;

        if (sftp.Exists(filePath))
            imageBytes = sftp.ReadAllBytes(filePath);

        return imageBytes;
    }

    public byte[] CompressImage(int jpegQuality, byte[] data)
    {
        if (!OperatingSystem.IsWindows())
            throw new NotSupportedException("CompressImage function only works on windows platform");

        using var inputStream = new MemoryStream(data);
        using var image = Image.FromStream(inputStream);

        var jpegEncoder = ImageCodecInfo.GetImageDecoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid);
        var encoderParameters = new EncoderParameters(1);
        encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, jpegQuality);

        using var outputStream = new MemoryStream();
        image.Save(outputStream, jpegEncoder, encoderParameters);

        return outputStream.ToArray();
    }

    private ConnectionInfo GetConnectionInfo()
    {
        return new ConnectionInfo(_configSettings.SftpSettings.Ip, _configSettings.SftpSettings.UserName,
            new PasswordAuthenticationMethod(_configSettings.SftpSettings.UserName,
                _configSettings.SftpSettings.Password));
    }
}