using BLL.Abstract;

namespace API.Graphql.Role;

public class Query
{
    [UseProjection]
    [UseSorting]
    [UseFiltering]
    public async Task<IQueryable<ENTITIES.Entities.Role>> GetRolesAsync(
        [Service] IRoleService service)
    {
        return (await service.GraphQlGetAsync()).Data;
    }
}