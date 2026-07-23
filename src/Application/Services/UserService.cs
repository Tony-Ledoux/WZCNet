using System;
using Microsoft.AspNetCore.Identity;
using WZCNet.src.Application.DTOs;
using WZCNet.src.Application.DTOs.Requests.Auth;
using WZCNet.src.Application.Interfaces;
using WZCNet.src.Application.Interfaces.Repositories;
using WZCNet.src.Domain.Common;
using WZCNet.src.Domain.Entities;
using WZCNet.src.Domain.Interfaces;

namespace WZCNet.src.Application.Services;

public class UserService(IUserRepository repo, IUnitOfWork db_actions, ITokenService _ts) : IUserService
{
    public string HashPassword(string password)
    {
        var passwordHasher = new PasswordHasher<AppUser>();
        return passwordHasher.HashPassword(null,password);
    }

    public async Task<Result<string>> Login(LoginRequestDto requestDto)
    {
        var user = await repo.GetAppuserByUserName(requestDto.UserName);
        if(user == null) return Result<string>.Failure("Gebruiker bestaat niet");
        //check the password
        var passwordHasher = new PasswordHasher<AppUser>();
        if(passwordHasher.VerifyHashedPassword(user,user.PasswordHash,requestDto.Password) == PasswordVerificationResult.Failed) return Result<string>.Failure("Ongeldig wachtwoord");
        user.LastLogin = DateTime.UtcNow;
        // create Jwt
        var ts = new TokenClaimsDTO {
                UserName = user.UserName
            };
        string token = await _ts.CreateBearerToken(ts);
        await db_actions.SaveChangesAsync();
        return Result<string>.Success(token);
    }

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

    

    public bool VerifyPassword(string userName, string password)
    {
        throw new NotImplementedException();
    }
}
