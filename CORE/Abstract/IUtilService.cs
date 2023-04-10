﻿using DTO.Helper;

namespace CORE.Abstract;

public interface IUtilService
{
    public bool IsValidToken();
    public PaginationDto GetPagination();
    public int? GetUserIdFromToken();
    public int? GetCompanyIdFromToken();
    public string GenerateRefreshToken();
    public string GetTokenStringFromHeader(string jwtToken);
    public string Encrypt(string value);
    public string Decrypt(string value);
    public Task SendMail(string email, string message);
    public string CreateGuid();
    string GetEnvFolderPath(string folderName);
}