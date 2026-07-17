using System;

namespace WZCNet.Models;

public class EmployeeAddressDto
{
    public int Id {get;set;}
    public int EmployeeId {get;set;}
    public string StreetName {get;set;}
    public string HouseNumber {get;set;}
    public string ZipCode {get;set;}
    public string Municipality {get;set;}
    public DateOnly? Until {get;set;}
}
