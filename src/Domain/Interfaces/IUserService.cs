using System;
using WZCNet.src.Application.DTOs.Requests.Auth;
using WZCNet.src.Domain.Entities;

namespace WZCNet.src.Domain.Interfaces;

public interface IUserService
{
    AppUser Register(LoginRequestDto request);
    bool VerifyPassword(AppUser user, string password);
}
