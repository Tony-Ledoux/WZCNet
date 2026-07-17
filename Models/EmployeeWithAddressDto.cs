using System;

namespace WZCNet.Models;

public class EmployeeWithAddressDto:EmployeeBaseDto
{
    public List<EmployeeAddressDto> Addresses {get;set;}= [];
}
