using BLL.Abstract;
using DTO.Role;

namespace API.Graphql.Role;

public class Mutation
{
    /*
query GetRoles{
  roles {
    roleId,
    name
  }
}

mutation AddRole{
  addRole(item: {name: "sdsds", key: "TYPE1"}) { # mutation name
    roleId,
    name
  }
}
     */

    public async Task<bool> AddRole(RoleToAddDto item, [Service] IRoleService service)
    {
        await service.AddAsync(item);
        return true;
        //return ().Data;
    }

    public async Task<bool> UpdateRole(int id, RoleToUpdateDto item, [Service] IRoleService service)
    {
        await service.UpdateAsync(id, item);

        return true;
        //return ().Data;
    }
}