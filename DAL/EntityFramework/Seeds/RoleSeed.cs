using CORE.Enums;
using CORE.Helpers;
using ENTITIES.Entities;
using Microsoft.EntityFrameworkCore;
namespace DAL.EntityFramework.Seeds;

public static class RoleSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        List<Guid> roleIds = new()
        {
            Guid.Parse("a603330a-c6f1-4763-998d-e8d3c909032b"),
            Guid.Parse("33a8ba44-1e5e-4cc2-905c-be8a6c208590"),
            Guid.Parse("3d3b766a-aeec-43d1-9e69-f1c2b3eb9100"),
            Guid.Parse("41b90ae8-5629-4914-8a45-73c48fa03ec5")
        };

        var roles = Enum.GetValues<EUserType>()
            .Select((e, index) => new Role
            {
                Id = roleIds[index],
                Key = Enum.GetName(e)!,
                Name = EnumHelper.GetEnumDescription(e)
            })
            .ToArray();

        modelBuilder.Entity<Role>().HasData(roles);
    }
}