using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Identity;
using WZCNet.src.Application.DTOs;
using WZCNet.src.Application.DTOs.Requests.Auth;
using WZCNet.src.Application.DTOs.Responses;
using WZCNet.src.Application.Interfaces;
using WZCNet.src.Application.Interfaces.Repositories;
using WZCNet.src.Domain.Common;
using WZCNet.src.Domain.Entities;
using WZCNet.src.Domain.Interfaces;
using WZCNet.src.Domain.ValueObjects;

namespace WZCNet.src.Application.Services;

public class UserService(IUserRepository repo, IUnitOfWork db_actions, ITokenService _ts, IRequestContext rc) : IUserService
{
    public string HashPassword(string password)
    {
        var passwordHasher = new PasswordHasher<AppUser>();
        return passwordHasher.HashPassword(null,password);
    }

    public async Task<Result<LoginResponseDto>> Login(LoginRequestDto requestDto)
    {
        //TODO obscure the response on username/password error
        var user = await repo.GetAppuserByUserName(requestDto.UserName);
        if(user == null) return Result<LoginResponseDto>.Failure("Gebruiker bestaat niet");
        //check the password
        var passwordHasher = new PasswordHasher<AppUser>();
        if(passwordHasher.VerifyHashedPassword(user,user.PasswordHash,requestDto.Password) == PasswordVerificationResult.Failed) return Result<LoginResponseDto>.Failure("Ongeldig wachtwoord");
        user.LastLogin = DateTime.UtcNow;
        // create Jwt
        var ts = new TokenClaimsDTO {
                UserName = user.UserName
            };
        string token = await _ts.CreateBearerToken(ts);
        //create a refreshtoken
        
        var rf = Refreshtoken.GetRefreshtoken(user.Id,SessionInfo.Create(rc.DeviceInfo,rc.IpAddress));
        user.Refreshtokens.Add(rf);
        
        await db_actions.SaveChangesAsync();

        return Result<LoginResponseDto>.Success(new LoginResponseDto{AccessToken=token,RefreshToken=rf.RefreshToken});
    }

    /*
    public async Task<Result<LoginResponseDto>> Refresh(RefreshRequestDto request)
    {
        var user = await repo.GetAppUserByIdAsync(request.AccountId);
        if(user == null) return Result<LoginResponseDto>.Failure("Geen gekende gebruiker"); 
    }
    */
    public async Task<Result<AppUser>> Register(LoginRequestDto request)
    {
        // check for duplicate
        if(await repo.UserExists(request.UserName)) return Result<AppUser>.Failure("gebruiker bestaat al");
        var user = AppUser.Create(request.UserName,HashPassword(request.Password),request.IsPersonalAccount ?? false);
        if(!user.IsSuccess) return user;

        // save to the database
        await repo.AddUserToDatabase(user.Value);
        await db_actions.SaveChangesAsync();
        return user;

    }

}
