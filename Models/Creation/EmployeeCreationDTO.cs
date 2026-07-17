using System;
using System.ComponentModel.DataAnnotations;
using WZCNet.Validation;

namespace WZCNet.Models.Creation;

public class EmployeeCreationDTO
{
    private string _firstname = string.Empty;
    private string _lastname = string.Empty;

    [Required(ErrorMessage = "Voornaam is verplicht")]
    [MaxLength(50, ErrorMessage = "De maximumlengte is 50 characters")]
    public string FirstName
    {
        get => _firstname;
        set => _firstname = value?.Trim() ?? string.Empty;
    }

    [Required(ErrorMessage = "Familienaam is verplicht")]
    [MaxLength(50, ErrorMessage = "De maximumlengte is 50 characters")]
    public string LastName
    {
        get => _lastname;
        set => _lastname = value?.Trim() ?? string.Empty;
    }

    [Required]
    [DateRange(MaxYearsAgo = 120)]
    public DateOnly DateOfBirth { get; set; } // must be younger than 120 years cannot be in the future

}
