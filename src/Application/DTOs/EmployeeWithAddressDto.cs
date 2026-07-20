

namespace WZCNet.src.Application.DTOs;

public class EmployeeWithAddressDto:EmployeeBaseDto
{
    public List<EmployeeAddressDto> Addresses {get;set;}= [];
}
