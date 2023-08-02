using CORE.Abstract;
using DAL.EntityFramework.Seeds;
using ENTITIES.Entities;
using ENTITIES.Entities.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using File = ENTITIES.Entities.File;

namespace DAL.EntityFramework.Context;

public class DataContext : DbContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUtilService _utilService;

    public DataContext(DbContextOptions<DataContext> options,
        IHttpContextAccessor httpContextAccessor,
        IUtilService utilService) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
        _utilService = utilService;
    }

    public required DbSet<User> Users { get; set; }
    public required DbSet<File> Files { get; set; }
    public required DbSet<Organization> Organizations { get; set; }
    public required DbSet<Role> Roles { get; set; }
    public required DbSet<RequestLog> RequestLogs { get; set; }
    public required DbSet<ResponseLog> ResponseLogs { get; set; }
    public required DbSet<Permission> Permissions { get; set; }
    public required DbSet<Token> Tokens { get; set; }

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
      dotnet ef --startup-project ../API migrations add initial --context DataContext
      dotnet ef --startup-project ../API database update --context DataContext
    */

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AddGlobalFilter("IsDeleted", false);

        modelBuilder.Entity<Token>().HasQueryFilter(m => !m.IsDeleted);

        DataSeed.Seed(modelBuilder);
    }

    private void SetAuditProperties()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is Auditable && e.State is EntityState.Added or EntityState.Modified);

        foreach (var entityEntry in entries)
            switch (entityEntry.State)
            {
                case EntityState.Added:
                    // var originalValues = entityEntry.OriginalValues.ToObject();
                    // var currentValues = entityEntry.CurrentValues.ToObject();
                    ((Auditable)entityEntry.Entity).CreatedAt = DateTime.Now;
                    ((Auditable)entityEntry.Entity).CreatedById =
                        _utilService.GetUserIdFromToken();
                    break;
                case EntityState.Modified:
                {
                    Entry((Auditable)entityEntry.Entity).Property(p => p.CreatedAt).IsModified =
                        false;
                    Entry((Auditable)entityEntry.Entity).Property(p => p.CreatedById).IsModified =
                        false;

                    if (((Auditable)entityEntry.Entity).IsDeleted)
                    {
                        Entry((Auditable)entityEntry.Entity).Property(p => p.ModifiedBy)
                            .IsModified = false;
                        Entry((Auditable)entityEntry.Entity).Property(p => p.ModifiedAt)
                            .IsModified = false;

                        ((Auditable)entityEntry.Entity).DeletedAt = DateTime.Now;
                        ((Auditable)entityEntry.Entity).DeletedBy =
                            _utilService.GetUserIdFromToken();
                    }
                    else
                    {
                        ((Auditable)entityEntry.Entity).ModifiedAt = DateTime.Now;
                        ((Auditable)entityEntry.Entity).ModifiedBy =
                            _utilService.GetUserIdFromToken();
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