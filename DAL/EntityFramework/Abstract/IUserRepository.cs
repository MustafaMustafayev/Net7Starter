using DAL.EntityFramework.GenericRepository;
using ENTITIES.Entities;

namespace DAL.EntityFramework.Abstract;

public interface IUserRepository : IGenericRepository<User>
{
    Task<bool> IsUserExistAsync(string email, int? userId);

    Task<string?> GetUserSaltAsync(string userEmail);

    Task UpdateUserAsync(User user);
}