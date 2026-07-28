using System;
using Microsoft.EntityFrameworkCore;
using WZCNet.src.Application.Interfaces.Repositories;
using WZCNet.src.Domain.Common;
using WZCNet.src.Domain.Entities;
using WZCNet.src.Infrastructure.Persistence.Contexts;

namespace WZCNet.src.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository(WZCNetDbContext context) : IRefreshtokenRepository
{
    public Task<Refreshtoken?> GetRefreshtokenByTokenStringAsync(string token)
    {
        return context.Refreshtokens.Include(t=>t.AppUser).Include(t=>t.Employee).FirstOrDefaultAsync(t=>t.RefreshToken == token);
    }
}
