namespace WZCNet.src.Application.DTOs.Requests.Auth;
public class LoginRequestDto
{
    public string UserName {get;set;}=string.Empty;
    public string Password {get;set;}=string.Empty;
    public bool? IsPersonalAccount {get;set;}
}