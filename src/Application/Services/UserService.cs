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
    IUserAccountRepository userRepo,
    IEmployeeRepository employeeRepo,
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
                UserAccountId = user.Id,
                EmployeeName = employee?.GetName(),
                EmployeeId = employee?.Id,
                RequiresPinChange = employee?.Pin?.PinChangedAt.HasValue != true
            };
        return await _ts.CreateBearerToken(ts);
    }

    private async Task<string> HandleToken(int accountId, SessionInfo session,int? employeeId)
    {
        var token = await userRepo.GetRefreshtokenByAccountIdAndSessionInfoAsync(accountId,session);
        if(token == null)
        {
            var t = Refreshtoken.Create(accountId,session,employeeId);
            dbActions.Add(t);
            return t.RefreshToken;
        }
        // check if expirationdate is within 24 hours
        if(token.ValidUntil <= DateTime.UtcNow.AddHours(24))
        {
            dbActions.Remove(token);
            var t = Refreshtoken.Create(accountId,session,employeeId);
            dbActions.Add(t);
            return t.RefreshToken;
        }

        return token.RefreshToken;

    }

    private async Task<Employee?> GetEmployeeByIdFromUser(AppUser user, int? employeeId)
    {
        if(user == null) return null;
        if(user.IsPersonalAccount && user.Employees.Count == 1) return user.Employees.First();
        if(employeeId.HasValue) return user.Employees.FirstOrDefault(e=> e.Id == employeeId.Value);
        return null;
    }

    public async Task<Result<LoginResponseDto>> Login(LoginRequestDto requestDto)
    {
        var user = await userRepo.GetAppuserByUserName(requestDto.UserName);
        if(user == null) return Result<LoginResponseDto>.Failure("Ongeldige gebruikersnaam of wachtwoord");
        if(!user.IsActive) return Result<LoginResponseDto>.Failure("Account is geblokkeerd");
        //check the password
        if(_passwordHasher.VerifyHashedPassword(user,user.PasswordHash,requestDto.Password) == PasswordVerificationResult.Failed)
        {
            user.IncrementNumberOfFailedLoginAttempts();
            await dbActions.SaveChangesAsync();
            return Result<LoginResponseDto>.Failure("Ongeldige gebruikersnaam of wachtwoord");
        }
        //check if there is a refreshtoken
        var sessionInfo = SessionInfo.Create(rc.DeviceInfo, rc.IpAddress);
        var employeeId = user.GetSingleEmployeeId();
        var employee = await GetEmployeeByIdFromUser(user, employeeId);
        var response = new LoginResponseDto();
        user.RegisterSuccessfulLogin();
        response.RefreshToken = await HandleToken(user.Id,sessionInfo,employee?.Id);
        response.AccessToken = await CreateJWT(user,employee);
        await dbActions.SaveChangesAsync();
        return Result<LoginResponseDto>.Success(response);
    }


    public async Task<Result<LoginResponseDto>> Refresh(RefreshRequestDto request)
    {
        var token = await userRepo.GetRefreshtokenByTokenStringAsync(request.RefreshToken);
        if(token == null) return Result<LoginResponseDto>.Failure("Geen token gevonden");
        if(!token.IsValid())return Result<LoginResponseDto>.Failure("Token is vervallen");
        if(token.AppUserId != request.AccountId) return Result<LoginResponseDto>.Failure("Token is niet van deze gebruiker");
        var sessionInfo = SessionInfo.Create(rc.DeviceInfo, rc.IpAddress);
        var newToken = Refreshtoken.Create(token.AppUserId,sessionInfo,token?.EmployeeId);
        dbActions.Add(newToken);
        dbActions.Remove(token);
        await dbActions.SaveChangesAsync();
        var response = new LoginResponseDto
        {
            AccessToken = await CreateJWT(token.AppUser,token.Employee),
            RefreshToken = newToken.RefreshToken
        };
        return Result<LoginResponseDto>.Success(response);
    }

    public async Task<Result<AppUser>> Register(LoginRequestDto request)
    {
        // check for duplicate
        if(await userRepo.UserExists(request.UserName)) return Result<AppUser>.Failure("gebruiker bestaat al");
        var user = AppUser.Create(request.UserName,HashPassword(request.Password),request.IsPersonalAccount ?? false);
        if(!user.IsSuccess) return user;

        // save to the database
        dbActions.Add(user.Value);
        await dbActions.SaveChangesAsync();
        return user;

    }

    public async Task<Result<LoginResponseDto>> Identify(int accountId, IdentifyRequestDto request)
    {
        var user = await userRepo.GetAppUserByIdAsync(accountId);
        if(user == null) return Result<LoginResponseDto>.Failure("Account bestaat niet.");
        var employee = await GetEmployeeByIdFromUser(user, request.EmployeeId);
        if(employee == null) return Result<LoginResponseDto>.Failure("Werknemer bestaat niet");
        if(employee.Pin == null || !employee.Pin.ValidatePin(request.Pin)) return Result<LoginResponseDto>.Failure("Geen of onjuiste pin");
        //get the current refreshtoken
        var sessionInfo = SessionInfo.Create(rc.DeviceInfo, rc.IpAddress);
        var refreshtoken = await userRepo.GetRefreshtokenByAccountIdAndSessionInfoAsync(accountId, sessionInfo);
        if(refreshtoken != null)
        {
            refreshtoken.AttachEmployee(employee);
            await dbActions.SaveChangesAsync();
        }else
        {
            refreshtoken = Refreshtoken.Create(accountId,sessionInfo,employee.Id);
            dbActions.Add(refreshtoken);
            await dbActions.SaveChangesAsync();
        }
        var response = new LoginResponseDto
        {
            RefreshToken = refreshtoken.RefreshToken,
            AccessToken= await CreateJWT(user,employee)
        };
        return Result<LoginResponseDto>.Success(response);
    }

    public async Task<Result<AppUser>> AddEmployeeToUser(int employeeId, int userId)
    {

        var user = await userRepo.GetAppUserByIdAsync(userId);
        var employee = await employeeRepo.GetEmployeeByIdAsync(employeeId);
        if(user == null) return Result<AppUser>.Failure("Geen gebruiker gevonden");
        if(employee == null) return Result<AppUser>.Failure("Geen Werknemer gevonden");
        var result = user.AddEmployee(employee);
        if (!result.IsSuccess) return Result<AppUser>.Failure(result.Error);
        await dbActions.SaveChangesAsync();
        return Result<AppUser>.Success(user);
    }

    public async Task<Result<AppUser>> RemoveEmployeeFromUser(int employeeId, int userId)
    {
        var user = await userRepo.GetAppUserByIdAsync(userId);
        var employee = await employeeRepo.GetEmployeeByIdAsync(employeeId);
        if(user == null) return Result<AppUser>.Failure("Geen gebruiker gevonden");
        if(employee == null) return Result<AppUser>.Failure("Geen Werknemer gevonden");
        var result = user.RemoveEmployee(employee);
        if (!result.IsSuccess) return Result<AppUser>.Failure(result.Error);
        await dbActions.SaveChangesAsync();
        return Result<AppUser>.Success(user);
    }
}
