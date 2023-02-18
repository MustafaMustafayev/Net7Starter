using DAL.Abstract;
using DAL.DatabaseContext;
using DAL.UnitOfWorks.Abstract;

namespace DAL.UnitOfWorks.Concrete;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _dataContext;

    private bool _isDisposed;

    public UnitOfWork(
        DataContext dataContext,
        IUserRepository userRepository,
        ILoggingRepository loggingRepository,
        IRoleRepository roleRepository,
        IOrganizationRepository organizationRepository,
        IPermissionRepository permissionRepository,
        ITokenRepository tokenRepository)
    {
        _dataContext = dataContext;
        UserRepository = userRepository;
        LoggingRepository = loggingRepository;
        RoleRepository = roleRepository;
        OrganizationRepository = organizationRepository;
        PermissionRepository = permissionRepository;
        TokenRepository = tokenRepository;
    }

    public IUserRepository UserRepository { get; }
    public ILoggingRepository LoggingRepository { get; }
    public IRoleRepository RoleRepository { get; }
    public IOrganizationRepository OrganizationRepository { get; }
    public IPermissionRepository PermissionRepository { get; }
    public ITokenRepository TokenRepository { get; }

    public async Task CommitAsync()
    {
        await _dataContext.SaveChangesAsync();
    }

    public async ValueTask DisposeAsync()
    {
        if (!_isDisposed)
        {
            _isDisposed = true;
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }
    }

    public void Dispose()
    {
        if (_isDisposed) return;
        _isDisposed = true;
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing) _dataContext.Dispose();
    }

    private async ValueTask DisposeAsync(bool disposing)
    {
        if (disposing) await _dataContext.DisposeAsync();
    }
}