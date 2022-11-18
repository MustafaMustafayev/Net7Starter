namespace CORE.Abstract;

public interface IUtilService
{
    HttpContent GetHttpContentObject(object obj);
    public bool IsValidToken(string tokenString);

    public int? GetUserIdFromToken(string? tokenString);
    public int? GetCompanyIdFromToken(string? tokenString);

    public void AddTokenToCache(string token, DateTime expireDate);
    public bool IsTokenExistsInCache(string? token);
}