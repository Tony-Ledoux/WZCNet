using System.ComponentModel.DataAnnotations;

namespace WZCNet.src.Application.DTOs.Requests.User;
public class AddEmployeeRequestDto
{
    [Required]
    public int EmployeeId {get;set;}
}