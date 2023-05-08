using DAL.Abstract;
using DAL.DatabaseContext;
using DAL.GenericRepositories.Concrete;
using ENTITIES.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Concrete;

public class TokenRepository : GenericRepository<Token>, ITokenRepository
{
    private readonly DataContext _dataContext;

    public TokenRepository(DataContext dataContext)
        : base(dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<bool> IsValid(string accessToken, string refreshToken)
    {
        return await _dataContext.Tokens.AnyAsync(m => m.AccessToken == accessToken);
    }

    public async Task<List<Token>> GetActiveTokensAsync(string accessToken)
    {
        return await _dataContext.Tokens.Where(m => m.AccessToken == accessToken).ToListAsync();
    }
}