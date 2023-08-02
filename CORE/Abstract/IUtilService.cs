using DTO.Helper;
using ENTITIES.Enums;

namespace CORE.Abstract;

public interface IUtilService
{
    public string? GetTokenString();
    public bool IsValidToken();
    public PaginationDto GetPagination();
    public int? GetUserIdFromToken();
    public int? GetCompanyIdFromToken();
    public string GenerateRefreshToken();
    public string? GetRoleFromToken(string jwtToken);
    public string GetTokenStringFromHeader(string jwtToken);
    public string Encrypt(string value);
    public string Decrypt(string value);
    public Task SendMailAsync(string email, string message);
    public string CreateGuid();
    public string GetFolderName(FileType type);
}