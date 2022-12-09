using DAL.Abstract;
using DAL.DatabaseContext;
using DAL.GenericRepositories.Concrete;
using ENTITIES.Entities;

namespace DAL.Concrete;

public class PermissionRepository : GenericRepository<Permission>, IPermissionRepository
{
    private readonly DataContext _dataContext;

    public PermissionRepository(DataContext dataContext)
        : base(dataContext)
    {
        _dataContext = dataContext;
    }
}