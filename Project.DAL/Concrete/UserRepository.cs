﻿using Microsoft.EntityFrameworkCore;
using Project.DAL.Abstract;
using Project.DAL.DatabaseContext;
using Project.DAL.GenericRepositories.Concrete;
using Project.Entity.Entities;

namespace Project.DAL.Concrete;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly DataContext _dataContext;

    public UserRepository(DataContext dataContext)
        : base(dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<string?> GetUserSaltAsync(string userEmail)
    {
        var user = await _dataContext.Users.SingleOrDefaultAsync(m => m.Email == userEmail);

        return user == null ? null : user.Salt;
    }

    public async Task<bool> IsUserExistAsync(string userName, int? userId)
    {
        return await _dataContext.Users.AnyAsync(m => m.Username == userName && m.Id != userId);
    }

    public Task UpdateUserAsync(User user)
    {
        _dataContext.Entry(user).State = EntityState.Modified;
        _dataContext.Entry(user).Property(m => m.Password).IsModified = false;
        _dataContext.Entry(user).Property(m => m.Salt).IsModified = false;

        return Task.FromResult(1);
    }
}