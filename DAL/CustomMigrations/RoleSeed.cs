using ENTITIES.Entities;
using ENTITIES.Enums;
using Microsoft.EntityFrameworkCore;

namespace DAL.CustomMigrations;

public static class RoleSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
            new Role
            {
                RoleId = 1,
                Name = "Admin",
                Key = UserType.Admin.ToString()
            },
            new Role
            {
                RoleId = 2,
                Name = "Viewer",
                Key = UserType.Viewer.ToString()
            }
        );
    }
}