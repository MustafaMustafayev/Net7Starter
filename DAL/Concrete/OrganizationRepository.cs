using DAL.Abstract;
using DAL.DatabaseContext;
using DAL.GenericRepositories.Concrete;
using ENTITIES.Entities;

namespace DAL.Concrete;

public class OrganizationRepository : GenericRepository<Organization>, IOrganizationRepository
{
    private readonly DataContext _dataContext;

    public OrganizationRepository(DataContext dataContext)
        : base(dataContext)
    {
        _dataContext = dataContext;
    }
}