using DAL.GenericRepositories.Abstract;
using ENTITIES.Entities;

namespace DAL.Abstract;

public interface IUserRepository : IGenericRepository<User>
{
    Task<bool> IsUserExistAsync(string email, int? userId);

    Task<string?> GetUserSaltAsync(string userEmail);

    Task UpdateUserAsync(User user);
}