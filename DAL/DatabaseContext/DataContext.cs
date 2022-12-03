using System.Linq.Expressions;
using CORE.Abstract;
using DAL.CustomMigrations;
using DTO.User;
using ENTITIES.Entities;
using ENTITIES.Entities.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DAL.DatabaseContext;

public class DataContext : DbContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUtilService _utilService;

    public DataContext(DbContextOptions<DataContext> options, IHttpContextAccessor httpContextAccessor,
        IUtilService utilService) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
        _utilService = utilService;
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Photo> Photos { get; set; }

    public DbSet<Organization> Organizations { get; set; }

    public DbSet<Role> Roles { get; set; }
    public DbSet<RequestLog> RequestLogs { get; set; }

    public DbSet<ResponseLog> ResponseLogs { get; set; }

    public DbSet<ENTITIES.Entities.Logging.NLog> NLogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetAuditProperties();
        return await base.SaveChangesAsync(cancellationToken);
    }

    /* migration commands
      dotnet ef --startup-project ../API migrations add redisentities --context DataContext
      dotnet ef --startup-project ../API database update --context DataContext
    */

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            if (typeof(Auditable).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "p");
                var deletedCheck = Expression.Lambda(
                    Expression.Equal(
                        Expression.Property(parameter, "IsDeleted"),
                        Expression.Constant(false)
                    ), parameter);
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(deletedCheck);
            }

        DataSeed.Seed(modelBuilder);
    }

    private void SetAuditProperties()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is Auditable && e.State is EntityState.Added or EntityState.Modified);

        var tokenString = _httpContextAccessor?.HttpContext?.Request.Headers["Authorization"].ToString();
        foreach (var entityEntry in entries)
            switch (entityEntry.State)
            {
                case EntityState.Added:
                    // var originalValues = entityEntry.OriginalValues.ToObject();
                    // var currentValues = entityEntry.CurrentValues.ToObject();
                    ((Auditable)entityEntry.Entity).CreatedAt = DateTime.Now;
                    ((Auditable)entityEntry.Entity).CreatedBy = _utilService.GetUserIdFromToken(tokenString);
                    break;
                case EntityState.Modified:
                {
                    Entry((Auditable)entityEntry.Entity).Property(p => p.CreatedAt).IsModified = false;
                    Entry((Auditable)entityEntry.Entity).Property(p => p.CreatedBy).IsModified = false;

                    if (((Auditable)entityEntry.Entity).IsDeleted)
                    {
                        Entry((Auditable)entityEntry.Entity).Property(p => p.ModifiedBy).IsModified = false;
                        Entry((Auditable)entityEntry.Entity).Property(p => p.ModifiedAt).IsModified = false;

                        ((Auditable)entityEntry.Entity).DeletedAt = DateTime.Now;
                        ((Auditable)entityEntry.Entity).DeletedBy = _utilService.GetUserIdFromToken(tokenString);
                    }
                    else
                    {
                        ((Auditable)entityEntry.Entity).ModifiedAt = DateTime.Now;
                        ((Auditable)entityEntry.Entity).ModifiedBy = _utilService.GetUserIdFromToken(tokenString);
                    }

                    break;
                }
                case EntityState.Detached:
                    break;
                case EntityState.Unchanged:
                    break;
                case EntityState.Deleted:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
    }
}