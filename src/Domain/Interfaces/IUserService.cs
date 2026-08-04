using System;
using WZCNet.src.Application.DTOs.Requests.Auth;
using WZCNet.src.Application.DTOs.Responses;
using WZCNet.src.Domain.Common;
using WZCNet.src.Domain.Entities;
using WZCNet.src.Domain.Entities.EmployeeAggregate;

namespace WZCNet.src.Domain.Interfaces;

public interface IUserService
{
    Task<Result<AppUser>> Register(LoginRequestDto request);

    Task<Result<LoginResponseDto>> Login(LoginRequestDto requestDto);
    
    Task<Result<LoginResponseDto>> Refresh(RefreshRequestDto request);
    
    string HashPassword(string password);

    Task<Result<LoginResponseDto>> Identify(int accountId, IdentifyRequestDto request);

    Task<Result<AppUser>> AddEmployeeToUser(int employeeId, int userId);
    Task<Result<AppUser>> RemoveEmployeeFromUser(int employeeId, int userId);


}
