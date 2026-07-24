namespace WZCNet.src.Application.DTOs.Requests.Auth;
public class RefreshRequestDto
{
    public string RefreshToken {get;set;}=string.Empty;
    public int AccountId {get;set;}
}