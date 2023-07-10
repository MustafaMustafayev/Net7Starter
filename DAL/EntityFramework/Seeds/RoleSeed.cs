using CORE.Helper;
using ENTITIES.Entities;
using ENTITIES.Enums;
using Microsoft.EntityFrameworkCore;

namespace DAL.EntityFramework.Seeds;

public static class RoleSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var roles = Enum.GetValues<UserType>()
            .Select(e => new Role
            {
                Id = (int)e,
                Key = Enum.GetName(e)!,
                Name = EnumHelper.GetEnumDescription(e)
            })
            .ToArray();

        modelBuilder.Entity<Role>().HasData(roles);
    }
}