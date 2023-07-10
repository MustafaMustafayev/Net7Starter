using DAL.EntityFramework.Abstract;
using DAL.EntityFramework.Context;

namespace DAL.EntityFramework.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _dataContext;

    private bool _isDisposed;

    public UnitOfWork(
        DataContext dataContext,
        IFileRepository fileRepository,
        INlogRepository nlogRepository,
        IOrganizationRepository organizationRepository,
        IPermissionRepository permissionRepository,
        IRequestLogRepository requestLogRepository,
        IResponseLogRepository responseLogRepository,
        IRoleRepository roleRepository,
        ITokenRepository tokenRepository,
        IUserRepository userRepository
    )
    {
        _dataContext = dataContext;
        FileRepository = fileRepository;
        NlogRepository = nlogRepository;
        OrganizationRepository = organizationRepository;
        PermissionRepository = permissionRepository;
        RequestLogRepository = requestLogRepository;
        ResponseLogRepository = responseLogRepository;
        RoleRepository = roleRepository;
        TokenRepository = tokenRepository;
        UserRepository = userRepository;
    }

    public IFileRepository FileRepository { get; set; }
    public INlogRepository NlogRepository { get; set; }
    public IOrganizationRepository OrganizationRepository { get; set; }
    public IPermissionRepository PermissionRepository { get; set; }
    public IRequestLogRepository RequestLogRepository { get; set; }
    public IResponseLogRepository ResponseLogRepository { get; set; }
    public IRoleRepository RoleRepository { get; set; }
    public ITokenRepository TokenRepository { get; set; }
    public IUserRepository UserRepository { get; set; }

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