using DAL.EntityFramework.Abstract;
using DAL.EntityFramework.Context;
using DAL.EntityFramework.GenericRepository;
using ENTITIES.Entities.Structures;

namespace DAL.EntityFramework.Concrete;

public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
{
    private readonly DataContext _dataContext;

    public DepartmentRepository(DataContext dataContext)
        : base(dataContext)
    {
        _dataContext = dataContext;
    }
}