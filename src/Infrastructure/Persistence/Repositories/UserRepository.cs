using System;
using Microsoft.EntityFrameworkCore;
using WZCNet.src.Application.Interfaces.Repositories;
using WZCNet.src.Domain.Common;
using WZCNet.src.Domain.Entities;
using WZCNet.src.Infrastructure.Persistence.Contexts;

namespace WZCNet.src.Infrastructure.Persistence.Repositories;

public class UserRepository(WZCNetDbContext context) : IUserRepository
{
 

    public async Task<AppUser?> GetAppuserByUserName(string userName)
    {
        return await context.AppUsers.FirstOrDefaultAsync(au=> au.UserName == userName);
    }

    public async Task<bool> UserExists(string userName)
    {
        return await context.AppUsers.AnyAsync(au=>au.UserName == userName);
    }

    public async Task<AppUser?> GetAppUserByIdAsync(int id)
    {
        // get user by Id 
        return await context.AppUsers.Include(u=>u.Refreshtokens).FirstOrDefaultAsync(u=>u.Id == id);
    }

}
