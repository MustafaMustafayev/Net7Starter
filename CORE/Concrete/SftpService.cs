using CORE.Abstract;
using CORE.Config;
using DTO.Sftp;
using Microsoft.AspNetCore.Http;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using System.Drawing;
using System.Drawing.Imaging;
using ConnectionInfo = Renci.SshNet.ConnectionInfo;

namespace CORE.Concrete
{
    public class SftpService : ISftpService
    {
        public ConfigSettings _configSettings;
        public SftpService(ConfigSettings configSettings)
        {
            _configSettings = configSettings;
        }
        public List<DirectoryInformation> GetDirectoryInformation(string path)
        {
            if (string.IsNullOrEmpty(path) || string.IsNullOrWhiteSpace(path))
            {
                path = string.Empty;
            }

            List<DirectoryInformation> directoryInfos = new List<DirectoryInformation>();
            var connectionInfo = GetConnectionInfo();
            using (var sftp = new SftpClient(connectionInfo))
            {
                sftp.Connect();
                if (sftp.IsConnected)
                {
                    string realPath = ("/" + path).Replace("//", "/");
                    sftp.ChangeDirectory(realPath);
                    List<SftpFile> directories = sftp.ListDirectory(realPath).ToList();
                    foreach (SftpFile sftpFile in directories)
                    {
                        string fileName = sftpFile.Name;
                        string[] fileParts = fileName.Split('.');
                        if (fileName != "." && fileName != "..")
                        {
                            bool isAvaliable = sftpFile.IsDirectory ? true : fileParts[fileParts.Length - 1] == "mp4" ? true : false;
                            if (isAvaliable)
                            {
                                string subDirectory = sftpFile.FullName.StartsWith("/") && sftpFile.FullName.Length > 0 ? sftpFile.FullName.Remove(0, 1) : sftpFile.FullName;

                                DirectoryInformation directoryInfo = new DirectoryInformation()
                                {
                                    Name = sftpFile.Name,
                                    Length = sftpFile.Length.ToString(),
                                    Path = subDirectory,
                                    CreatedAt = sftpFile.LastWriteTime.ToString("dd-MMM-yyyy HH:mm"),
                                    IsDirectory = sftpFile.IsDirectory
                                };
                                directoryInfos.Add(directoryInfo);
                            }
                        }
                    }
                }
            }

            return directoryInfos.OrderBy(m => m.Name).OrderBy(m => !m.IsDirectory).ToList();
        }

        public void UploadFile(string filePath, string fileName, IFormFile formFile)
        {
            var connectionInfo = GetConnectionInfo();
            using (var sftp = new SftpClient(connectionInfo))
            {
                sftp.Connect();
                if (sftp.IsConnected)
                {
                    sftp.ChangeDirectory(filePath);
                    using (var ms = new MemoryStream())
                    {
                        formFile.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        sftp.WriteAllBytes(fileName, fileBytes);
                    }

                    sftp.Disconnect();
                }
            }
        }

        private ConnectionInfo GetConnectionInfo()
        {
            return new Renci.SshNet.ConnectionInfo(_configSettings.SftpSettings.IP, _configSettings.SftpSettings.UserName, new PasswordAuthenticationMethod(_configSettings.SftpSettings.UserName, _configSettings.SftpSettings.Password));
        }

        public void DeleteFile(string filePath)
        {
            var connectionInfo = GetConnectionInfo();
            using (var sftp = new SftpClient(connectionInfo))
            {
                sftp.Connect();
                if (sftp.IsConnected)
                {
                    sftp.ChangeDirectory(filePath);


                    if (sftp.Exists(filePath))
                    {
                        sftp.Delete(filePath);
                    }
                }
            }
        }

        public byte[] ReadImage(string filePath)
        {
            byte[] imageBytes = new byte[] { };
            var connectionInfo = GetConnectionInfo();
            using (var sftp = new SftpClient(connectionInfo))
            {
                sftp.Connect();
                if (sftp.IsConnected)
                {
                    sftp.ChangeDirectory(filePath);

                    if (!string.IsNullOrEmpty(filePath))
                    {
                        if (sftp.Exists(filePath))
                        {
                            imageBytes = sftp.ReadAllBytes(filePath);
                        }
                    }
                }
            }
            return imageBytes;
        }

        public byte[] CompressImage(int jpegQuality, byte[] data)
        {
            using (MemoryStream inputStream = new MemoryStream(data))
            {
                using (Image image = Image.FromStream(inputStream))
                {
                    ImageCodecInfo jpegEncoder = ImageCodecInfo.GetImageDecoders()
                        .First(c => c.FormatID == ImageFormat.Jpeg.Guid);
                    var encoderParameters = new EncoderParameters(1);
                    encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, jpegQuality);
                    byte[] outputBytes = null;
                    using (MemoryStream outputStream = new MemoryStream())
                    {
                        image.Save(outputStream, jpegEncoder, encoderParameters);
                        return outputStream.ToArray();
                    }
                }
            }
        }
    }
}