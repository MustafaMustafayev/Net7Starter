using DAL.EntityFramework.Abstract;
using DAL.EntityFramework.Context;
using DAL.EntityFramework.GenericRepository;
using ENTITIES.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.EntityFramework.Concrete;

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