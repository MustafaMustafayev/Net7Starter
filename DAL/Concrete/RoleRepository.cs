using DAL.Abstract;
using DAL.DatabaseContext;
using DAL.GenericRepositories.Concrete;
using ENTITIES.Entities;

namespace DAL.Concrete;

public class RoleRepository : GenericRepository<Role>, IRoleRepository
{
    private readonly DataContext _dataContext;

    public RoleRepository(DataContext dataContext)
        : base(dataContext)
    {
        _dataContext = dataContext;
    }

    public Role UpdateRoleAsync(Role role)
    {
        return _dataContext.Roles.Update(role).Entity;
    }
}