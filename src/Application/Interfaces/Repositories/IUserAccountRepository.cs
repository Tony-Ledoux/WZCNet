using WZCNet.src.Domain.Entities;
using WZCNet.src.Domain.ValueObjects;

namespace WZCNet.src.Application.Interfaces.Repositories;

public interface IUserAccountRepository
{
    Task<bool> UserExists(string userName);

    Task<AppUser?> GetAppuserByUserName(string userName);
    Task<AppUser?> GetAppUserByIdAsync(int id);
    Task<Refreshtoken?> GetRefreshtokenByTokenStringAsync(string token);
    Task<Refreshtoken?> GetRefreshtokenByAccountIdAndSessionInfoAsync(int AccountId, SessionInfo session);
}
