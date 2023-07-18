using DAL.EntityFramework.Abstract;
using DAL.EntityFramework.Context;
using DAL.EntityFramework.GenericRepository;
using ENTITIES.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.EntityFramework.Concrete;

public class RoleRepository : GenericRepository<Role>, IRoleRepository
{
    private readonly DataContext _dataContext;

    public RoleRepository(DataContext dataContext)
        : base(dataContext)
    {
        _dataContext = dataContext;
    }

    public Role UpdateRole(Role role)
    {
        _dataContext.Roles.Update(role);
        return role;
    }

    public async Task ClearRolePermissionsAync(int roleId)
    {
        var entity = await _dataContext.Roles.FindAsync(roleId);
        entity!.Permissions!.Clear();
        _dataContext.Entry(entity).State = EntityState.Detached;
        await _dataContext.SaveChangesAsync();
    }
}