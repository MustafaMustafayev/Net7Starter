using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Project.DAL.DatabaseContext;

public static class ContextExtension
{
    public static void AddDatabaseContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<DataContext>(options => options.UseNpgsql(connectionString));
    }
}