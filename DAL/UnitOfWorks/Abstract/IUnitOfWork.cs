using DAL.Abstract;

namespace DAL.UnitOfWorks.Abstract;

public interface IUnitOfWork : IAsyncDisposable, IDisposable
{
    public IUserRepository UserRepository { get; }

    public ILoggingRepository LoggingRepository { get; }

    public IRoleRepository RoleRepository { get; }

    public IOrganizationRepository OrganizationRepository { get; }
    public IPermissionRepository PermissionRepository { get; }
    public ITokenRepository TokenRepository { get; }

    public Task CommitAsync();
}