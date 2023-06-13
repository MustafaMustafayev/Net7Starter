using DAL.EntityFramework.Abstract;
using DAL.EntityFramework.DatabaseContext;
using DAL.EntityFramework.GenericRepository;
using ENTITIES.Entities;

namespace DAL.EntityFramework.Concrete;

public class PermissionRepository : GenericRepository<Permission>, IPermissionRepository
{
    private readonly DataContext _dataContext;

    public PermissionRepository(DataContext dataContext)
        : base(dataContext)
    {
        _dataContext = dataContext;
    }
}