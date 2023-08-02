using DAL.EntityFramework.GenericRepository;
using ENTITIES.Entities;

namespace DAL.EntityFramework.Abstract;

public interface IRoleRepository : IGenericRepository<Role>
{
    Role UpdateRole(Role role);
    Task ClearRolePermissionsAync(int roleId);
}