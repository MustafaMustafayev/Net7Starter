using DAL.Abstract;
using DAL.DatabaseContext;
using DAL.GenericRepositories.Concrete;
using ENTITIES.Entities;

namespace DAL.Concrete;

public class AuthRepository : GenericRepository<User>, IAuthRepository
{
    private readonly DataContext _dataContext;

    public AuthRepository(DataContext dataContext)
        : base(dataContext)
    {
        _dataContext = dataContext;
    }
}