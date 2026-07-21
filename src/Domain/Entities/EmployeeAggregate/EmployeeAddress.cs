
using WZCNet.src.Domain.Common;

namespace WZCNet.src.Domain.Entities.EmployeeAggregate;

public class EmployeeAddress: BaseEntity
{
    public int EmployeeId {get; private set;}
    public string StreetName { get; private set;}
    public string HouseNumber { get; private set;}
    public string ZipCode { get; private set;}
    public string Municipality { get; private set; }
    public DateOnly? Until { get; private set; }
    public Employee  Employee {get;set;}
   
    private EmployeeAddress() { }

    public static Result<EmployeeAddress> Create(string streetName, string houseNumber, 
        string zipCode, string municipality, DateOnly? until)
    {
        ArgumentException.ThrowIfNullOrEmpty(streetName, nameof(streetName));
        ArgumentException.ThrowIfNullOrEmpty(houseNumber, nameof(houseNumber));
        ArgumentException.ThrowIfNullOrEmpty(zipCode, nameof(zipCode));
        ArgumentException.ThrowIfNullOrEmpty(municipality, nameof(municipality));

        if (until.HasValue)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            if (until.Value < today.AddYears(-100) || until.Value > today.AddYears(100))
                return Result<EmployeeAddress>.Failure("'Until' datum moet tussen 100 jaar geleden en 100 jaar in de toekomst liggen.");
        }
        return Result<EmployeeAddress>.Success(new EmployeeAddress 
        { 
            
            StreetName = streetName.Trim(),
            HouseNumber = houseNumber.Trim(),
            ZipCode = zipCode.Trim(),
            Municipality = municipality.Trim(),
            Until = until
        });
    }
}