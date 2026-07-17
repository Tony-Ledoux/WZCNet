using System;

namespace WZCNet.Models;

public class EmployeeBaseDto
{
    public int Id {get;set;}
    public string FirstName {get;set;}
    public string LastName {get;set;}
    public DateOnly DateOfBirth {get;set;}

    public int Age
    {
        get {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            int age = today.Year - DateOfBirth.Year;
            
            // Controleer of de verjaardag dit jaar al is geweest
            // Als de geboortedatum na "vandaag minus X jaar" ligt, trekken we er 1 van af
            if (DateOfBirth > today.AddYears(-age)) age--;
            
            return age;
        }}
}
