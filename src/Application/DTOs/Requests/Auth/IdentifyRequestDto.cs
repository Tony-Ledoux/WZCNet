using System.ComponentModel.DataAnnotations;

namespace WZCNet.src.Application.DTOs.Requests.Auth;
public class IdentifyRequestDto
{
    [Required]
    public int EmployeeId {get;set;}
    [Required]
    public string Pin {get;set;}=string.Empty;

}