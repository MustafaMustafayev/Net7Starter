using DAL.EntityFramework.GenericRepository;
using ENTITIES.Entities;

namespace DAL.EntityFramework.Abstract;

public interface IDepartmentRepository : IGenericRepository<Department>
{
}