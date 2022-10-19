using Project.DAL.Abstract;
using Project.DAL.DatabaseContext;
using Project.DAL.UnitOfWorks.Abstract;

namespace Project.DAL.UnitOfWorks.Concrete;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _dataContext;

    private bool _isDisposed;

    public UnitOfWork(
        DataContext dataContext,
        IUserRepository userRepository,
        IAuthRepository authRepository,
        ILoggingRepository loggingRepository,
        IRoleRepository roleRepository,
        IOrganizationRepository organizationRepository)
    {
        _dataContext = dataContext;
        UserRepository = userRepository;
        AuthRepository = authRepository;
        LoggingRepository = loggingRepository;
        RoleRepository = roleRepository;
        OrganizationRepository = organizationRepository;
    }

    public IUserRepository UserRepository { get; set; }

    public IAuthRepository AuthRepository { get; set; }

    public ILoggingRepository LoggingRepository { get; set; }

    public IRoleRepository RoleRepository { get; set; }

    public IOrganizationRepository OrganizationRepository { get; set; }

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

    protected async ValueTask DisposeAsync(bool disposing)
    {
        if (disposing) await _dataContext.DisposeAsync();
    }
}