using DAL.EntityFramework.GenericRepository;
using ENTITIES.Entities;

namespace DAL.EntityFramework.Abstract;

public interface IRoleRepository : IGenericRepository<Role>
{
    Role UpdateRole(Role role);
    Task AddRoleAsync(Role role);
    Task ClearRolePermissionsAync(Guid roleId);
}