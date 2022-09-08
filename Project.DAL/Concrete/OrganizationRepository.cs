using Project.DAL.Abstract;
using Project.DAL.DatabaseContext;
using Project.DAL.GenericRepositories.Concrete;
using Project.Entity.Entities;

namespace Project.DAL.Concrete
{
	public class OrganizationRepository : GenericRepository<Organization>, IOrganizationRepository
    {
        private readonly DataContext _dataContext;

        public OrganizationRepository(DataContext dataContext)
            : base(dataContext)
        {
            _dataContext = dataContext;
        }
    }
}

