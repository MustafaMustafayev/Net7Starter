using DAL.EntityFramework.GenericRepository;
using ENTITIES.Entities;

namespace DAL.EntityFramework.Abstract;

public interface ITokenRepository : IGenericRepository<Token>
{
    Task<bool> IsValid(string accessToken, string refreshToken);
    Task<List<Token>> GetActiveTokensAsync(string accessToken);
}