using CORE.Enums;
using CORE.Helpers;
using ENTITIES.Entities;
using Microsoft.EntityFrameworkCore;
namespace DAL.EntityFramework.Seeds;

public static class RoleSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var roles = Enum.GetValues<EUserType>()
            .Select(e => new Role
            {
                Id = Guid.NewGuid(),
                Key = Enum.GetName(e)!,
                Name = EnumHelper.GetEnumDescription(e)
            })
            .ToArray();

        modelBuilder.Entity<Role>().HasData(roles);
    }
}