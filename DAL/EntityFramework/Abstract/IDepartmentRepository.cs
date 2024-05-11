using DAL.EntityFramework.GenericRepository;
using ENTITIES.Entities.Structures;

namespace DAL.EntityFramework.Abstract;

public interface IDepartmentRepository : IGenericRepository<Department>
{
}