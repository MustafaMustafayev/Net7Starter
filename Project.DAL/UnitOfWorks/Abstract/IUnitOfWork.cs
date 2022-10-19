using Project.DAL.Abstract;

namespace Project.DAL.UnitOfWorks.Abstract;

public interface IUnitOfWork : IAsyncDisposable
{
    public IUserRepository UserRepository { get; set; }

    public IAuthRepository AuthRepository { get; set; }

    public ILoggingRepository LoggingRepository { get; set; }

    public IRoleRepository RoleRepository { get; set; }

    public IOrganizationRepository OrganizationRepository { get; set; }

    public Task CommitAsync();
}