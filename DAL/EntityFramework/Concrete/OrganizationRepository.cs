using DAL.EntityFramework.Abstract;
using DAL.EntityFramework.Context;
using DAL.EntityFramework.GenericRepository;
using ENTITIES.Entities;

namespace DAL.EntityFramework.Concrete;

public class OrganizationRepository : GenericRepository<Organization>, IOrganizationRepository
{
    private readonly DataContext _dataContext;

    public OrganizationRepository(DataContext dataContext)
        : base(dataContext)
    {
        _dataContext = dataContext;
    }
}