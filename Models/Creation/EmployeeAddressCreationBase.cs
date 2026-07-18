using System;
using System.ComponentModel.DataAnnotations;
using WZCNet.Validation;

namespace WZCNet.Models.Creation;

public class EmployeeAddressCreationBase
{
    private string _streetName = string.Empty;
    private string _houseNumber = string.Empty;
    private string _zipCode = string.Empty;
    private string _municipality = string.Empty;
    [Required(ErrorMessage ="Straatnaam mag niet leeg zijn")]
    public string StreetName
    {
        get => _streetName;
        set => _streetName = value?.Trim() ?? string.Empty;
    }
    [Required(ErrorMessage ="Huisnummer mag niet leeg zijn")]
    public string HouseNumber
    {
        get => _houseNumber;
        set => _houseNumber = value?.Trim() ?? string.Empty;
    }
    [Required(ErrorMessage ="Postcode mag niet leeg zijn")]
    public string ZipCode
    {
        get => _zipCode;
        set => _zipCode = value?.Trim() ?? string.Empty;
    }
    [Required(ErrorMessage ="Gemeente mag niet leeg zijn")]
    public string Municipality
    {
        get => _municipality;
        set => _municipality = value?.Trim() ?? string.Empty;
    }
    [DateRange(MaxYearsAgo =120, AllowFuture =true, MaxYearsFuture = 5)]
    public DateOnly? Until {get;set;}
}
