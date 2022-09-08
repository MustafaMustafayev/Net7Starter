using Project.DAL.Abstract;
using Project.DAL.DatabaseContext;
using Project.DAL.GenericRepositories.Concrete;
using Project.Entity.Entities;

namespace Project.DAL.Concrete;

public class AuthRepository : GenericRepository<User>, IAuthRepository
{
    private readonly DataContext _dataContext;

    public AuthRepository(DataContext dataContext)
        : base(dataContext)
    {
        _dataContext = dataContext;
    }
}