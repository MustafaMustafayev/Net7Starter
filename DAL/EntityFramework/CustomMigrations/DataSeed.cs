using Microsoft.EntityFrameworkCore;

namespace DAL.EntityFramework.CustomMigrations;

public static class DataSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        RoleSeed.Seed(modelBuilder);
        UserSeed.Seed(modelBuilder);
    }
}