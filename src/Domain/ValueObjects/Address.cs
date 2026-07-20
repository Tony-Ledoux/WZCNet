using WZCNet.src.Domain.Common;
using WZCNet.src.Domain.Interfaces;


namespace WZCNet.src.Domain.ValueObjects;

public sealed class Address: ISoftDeletable
{
    public string StreetName { get; private set;}
    public string HouseNumber { get; private set;}
    public string ZipCode { get; private set;}
    public string Municipality { get; private set; }
    public DateOnly? Until { get; private set; }
    public DateTime? DeletedAt { get ; set ; }

    private Address() { }

    public static Result<Address> Create(string streetName, string houseNumber, 
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
                return Result<Address>.Failure("'Until' datum moet tussen 100 jaar geleden en 100 jaar in de toekomst liggen.");
        }
        return Result<Address>.Success(new Address 
        { 
            StreetName = streetName.Trim(),
            HouseNumber = houseNumber.Trim(),
            ZipCode = zipCode.Trim(),
            Municipality = municipality.Trim(),
            Until = until
        });
    }
}