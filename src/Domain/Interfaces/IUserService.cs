using System;
using WZCNet.src.Application.DTOs.Requests.Auth;
using WZCNet.src.Domain.Common;
using WZCNet.src.Domain.Entities;

namespace WZCNet.src.Domain.Interfaces;

public interface IUserService
{
    Task<Result<AppUser>> Register(LoginRequestDto request);

    Task<Result<string>> Login(LoginRequestDto requestDto);
    string HashPassword(string password);
    bool VerifyPassword(string userName, string password);
}
