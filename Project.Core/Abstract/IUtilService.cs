namespace Project.Core.Abstract;

public interface IUtilService
{
    HttpContent GetHttpContentObject(object obj);

    public int? GetUserIdFromToken(string tokenString);

    public bool IsValidToken(string tokenString);

    public IEnumerable<string> GetFilterKeys();
}