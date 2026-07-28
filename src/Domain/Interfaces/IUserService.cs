using System;
using WZCNet.src.Application.DTOs.Requests.Auth;
using WZCNet.src.Application.DTOs.Responses;
using WZCNet.src.Domain.Common;
using WZCNet.src.Domain.Entities;

namespace WZCNet.src.Domain.Interfaces;

public interface IUserService
{
    Task<Result<AppUser>> Register(LoginRequestDto request);

    Task<Result<LoginResponseDto>> Login(LoginRequestDto requestDto);
    
    Task<Result<AppUser>> Refresh(RefreshRequestDto request);
    
    string HashPassword(string password);


}
