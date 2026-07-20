using System;

namespace WZCNet.src.Application.DTOs;

public class EmployeeAddressDto
{
    public string StreetName {get;set;}
    public string HouseNumber {get;set;}
    public string ZipCode {get;set;}
    public string Municipality {get;set;}
    public DateOnly? Until {get;set;}
}
