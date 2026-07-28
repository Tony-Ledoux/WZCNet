using Microsoft.AspNetCore.Identity;
using WZCNet.src.Application.DTOs;
using WZCNet.src.Application.DTOs.Requests.Auth;
using WZCNet.src.Application.DTOs.Responses;
using WZCNet.src.Application.Interfaces;
using WZCNet.src.Application.Interfaces.Repositories;
using WZCNet.src.Domain.Common;
using WZCNet.src.Domain.Entities;
using WZCNet.src.Domain.Entities.EmployeeAggregate;
using WZCNet.src.Domain.Interfaces;
using WZCNet.src.Domain.ValueObjects;

namespace WZCNet.src.Application.Services;

public class UserService(
    IUserRepository userrepo,
    IRefreshtokenRepository rfrepo,
    IUnitOfWork dbActions,
    ITokenService _ts,
    IRequestContext rc,
    IPasswordHasher<AppUser> _passwordHasher
    ) : IUserService
{

    public string HashPassword(string password)
    {
        return _passwordHasher.HashPassword(null,password);
    }

    private async Task<string> CreateJWT(AppUser user, Employee? employee)
    {
         var ts = new TokenClaimsDTO {
                UserName = user.UserName,
                EmployeeName = employee?.GetName(),
                EmployeeId = employee?.Id
            };
        return await _ts.CreateBearerToken(ts);
    }

    private async Task<string> HandleRefreshToken(AppUser user, SessionInfo session, int? employeeId, Refreshtoken? exisitingToken)
    {
        if(exisitingToken == null) return user.AddRefreshToken(session,employeeId).RefreshToken;
        if (exisitingToken.ValidUntil <= DateTime.UtcNow.AddHours(24))
    {
        exisitingToken.Invalidate();
        return user.AddRefreshToken(session, employeeId).RefreshToken;
    }
    return exisitingToken.RefreshToken;
    }

    public async Task<Result<LoginResponseDto>> Login(LoginRequestDto requestDto)
    {
        //TODO obscure the response on username/password error
        //TODO Fix logic to use refreshtokenrepository
        var user = await userrepo.GetAppuserByUserName(requestDto.UserName);
        if(user == null) return Result<LoginResponseDto>.Failure("Gebruiker bestaat niet");
        if(!user.IsActive) return Result<LoginResponseDto>.Failure("Account is geblokkeerd");
        //check the password
        if(_passwordHasher.VerifyHashedPassword(user,user.PasswordHash,requestDto.Password) == PasswordVerificationResult.Failed)
        {
            user.IncrementNumberOfFailedLoginAttempts();
            await dbActions.SaveChangesAsync();
            return Result<LoginResponseDto>.Failure("Ongeldig wachtwoord");
        }
        var response = new LoginResponseDto();
        user.RegisterSuccessfulLogin();
        var employee = user.GetEmployeeById();
        response.AccessToken = await CreateJWT(user,employee);
        var sessionInfo = SessionInfo.Create(rc.DeviceInfo, rc.IpAddress);
        var refresh = user.GetRefreshtokenByDeviceInfoAndEmployeeId(sessionInfo,employee?.Id);
        response.RefreshToken = await HandleRefreshToken(user,sessionInfo,employee?.Id,refresh);
        await dbActions.SaveChangesAsync();
        return Result<LoginResponseDto>.Success(response);
    }


    public async Task<Result<LoginResponseDto>> Refresh(RefreshRequestDto request)
    {
        var token = await rfrepo.GetRefreshtokenByTokenStringAsync(request.RefreshToken);
        if(token == null) return Result<LoginResponseDto>.Failure("Geen token gevonden");
        if(!token.IsValid())return Result<LoginResponseDto>.Failure("Token is vervallen");
        if(token.AppUserId != request.AccountId) return Result<LoginResponseDto>.Failure("Token is niet van deze gebruiker");
        var response = new LoginResponseDto
        {
            AccessToken = await CreateJWT(token.AppUser,token.Employee),
            RefreshToken = "must revalidate"
        };
        return Result<LoginResponseDto>.Success(response);
    }

    public async Task<Result<AppUser>> Register(LoginRequestDto request)
    {
        // check for duplicate
        if(await userrepo.UserExists(request.UserName)) return Result<AppUser>.Failure("gebruiker bestaat al");
        var user = AppUser.Create(request.UserName,HashPassword(request.Password),request.IsPersonalAccount ?? false);
        if(!user.IsSuccess) return user;

        // save to the database
        dbActions.Add(user.Value);
        await dbActions.SaveChangesAsync();
        return user;

    }

}
