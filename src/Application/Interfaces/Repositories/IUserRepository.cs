using System;
using System.Runtime.CompilerServices;
using WZCNet.src.Domain.Entities;

namespace WZCNet.src.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task AddUserToDatabase(AppUser user);
    Task<bool> UserExists(string userName);
}
