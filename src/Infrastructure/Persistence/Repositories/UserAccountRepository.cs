using System;
using Microsoft.EntityFrameworkCore;
using WZCNet.src.Application.Interfaces.Repositories;
using WZCNet.src.Domain.Common;
using WZCNet.src.Domain.Entities;
using WZCNet.src.Domain.ValueObjects;
using WZCNet.src.Infrastructure.Persistence.Contexts;

namespace WZCNet.src.Infrastructure.Persistence.Repositories;

public class UserAccountRepository(WZCNetDbContext context) : IUserAccountRepository
{
 

    public async Task<AppUser?> GetAppuserByUserName(string userName)
    {
        return await context.AppUsers.Include(au=>au.Employees).FirstOrDefaultAsync(au=> au.UserName == userName);
    }

    public async Task<bool> UserExists(string userName)
    {
        return await context.AppUsers.AnyAsync(au=>au.UserName == userName);
    }

    public async Task<AppUser?> GetAppUserByIdAsync(int id)
    {
        return await context.AppUsers.Include(u=>u.Refreshtokens).Include(u=>u.Employees).FirstOrDefaultAsync(u=>u.Id == id);
    }

    public async Task<Refreshtoken?> GetRefreshtokenByAccountIdAndSessionInfoAsync(int accountId, SessionInfo session)
    {
        return context.Refreshtokens.Include(t=>t.AppUser).FirstOrDefault(t=>t.AppUserId == accountId && t.Device.DeviceInfo == session.DeviceInfo && t.Device.IpAddress == session.IpAddress && t.ValidUntil > DateTime.UtcNow);
    }

    public Task<Refreshtoken?> GetRefreshtokenByTokenStringAsync(string token)
    {
        return context.Refreshtokens.Include(t=>t.AppUser).Include(t=>t.Employee).FirstOrDefaultAsync(t=>t.RefreshToken == token);
    }

}
