namespace WZCNet.src.Application.DTOs.Requests.Auth;
public class IdentifyRequestDto
{
    public required int EmployeeId {get;set;}
    public required string Pin {get;set;}=string.Empty;

}