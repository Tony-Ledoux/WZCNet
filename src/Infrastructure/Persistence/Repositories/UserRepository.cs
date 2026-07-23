using System;
using Microsoft.EntityFrameworkCore;
using WZCNet.src.Application.Interfaces.Repositories;
using WZCNet.src.Domain.Entities;
using WZCNet.src.Infrastructure.Persistence.Contexts;

namespace WZCNet.src.Infrastructure.Persistence.Repositories;

public class UserRepository(WZCNetDbContext context) : IUserRepository
{
    public async Task AddUserToDatabase(AppUser user)
    {
        context.AppUsers.Add(user);
    }

    public async Task<bool> UserExists(string userName)
    {
        return await context.AppUsers.AnyAsync(au=>au.UserName == userName);
    }
}
