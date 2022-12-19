using Microsoft.EntityFrameworkCore;

namespace DAL.CustomMigrations;

public static class DataSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        RoleSeed.Seed(modelBuilder);
    }
}