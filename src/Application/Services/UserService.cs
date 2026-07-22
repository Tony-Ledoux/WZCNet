using System;
using WZCNet.src.Application.DTOs.Requests.Auth;
using WZCNet.src.Domain.Entities;
using WZCNet.src.Domain.Interfaces;

namespace WZCNet.src.Application.Services;

public class UserService : IUserService
{
    public AppUser Register(LoginRequestDto request)
    {
        throw new NotImplementedException();
    }

    public bool VerifyPassword(AppUser user, string password)
    {
        throw new NotImplementedException();
    }
}
