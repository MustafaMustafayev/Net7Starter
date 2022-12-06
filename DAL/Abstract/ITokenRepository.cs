using DAL.GenericRepositories.Abstract;
using ENTITIES.Entities;

namespace DAL.Abstract;

public interface ITokenRepository : IGenericRepository<Token>
{
    Task<bool> IsValid(string accessToken, string refreshToken);
    Task<List<Token>> GetActiveTokensAsync(string accessToken);
}