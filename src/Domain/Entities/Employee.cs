using WZCNet.src.Domain.Common;
using WZCNet.src.Domain.ValueObjects;


namespace WZCNet.src.Domain.Entities;

public class Employee : BaseEntity
{
    public Name Name { get; private set; }
    public DateOnly DateOfBirth { get; set; }

    // navigation Property

    public EmployeeAuthentication? Pin { get; set; }

    public ICollection<Address> Addresses { get; private set; } = [];
    //public ICollection<EmployeeContact> EmployeeContacts { get; set; } = [];
    //public ICollection<EmploymentHistory> EmploymentHistories { get; set; } = [];

    //public ICollection<EmployeeComment> CommentsAuthored { get; set; } = [];
    //public ICollection<EmployeeComment> CommentsRecieved { get; set; } = [];
    //public ICollection<EmployeePermission> PersonalPermissions { get; set; } = [];

    private Employee() { }
    public static Result<Employee> Create(string firstName, string lastName, DateOnly dateOfBirth, string pinHash,IEnumerable<Address> addresses)
    {
        ArgumentException.ThrowIfNullOrEmpty(firstName, nameof(firstName));
        ArgumentException.ThrowIfNullOrEmpty(lastName, nameof(lastName));
        ArgumentException.ThrowIfNullOrWhiteSpace(pinHash, nameof(pinHash));
    

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var youngestAllowed = today.AddYears(-16);
        var oldestAllowed = today.AddYears(-120);

        if (dateOfBirth == default)
            return Result<Employee>.Failure("Ongeldige geboortedatum.");

        if (dateOfBirth < oldestAllowed || dateOfBirth > today)
            return Result<Employee>.Failure("Geboortedatum moet binnen de laatste 120 jaar liggen.");

        if (dateOfBirth > youngestAllowed)
            return Result<Employee>.Failure("Werknemer moet minstens 16 jaar oud zijn.");

        var nameResult = Name.Create(firstName, lastName);
        if (!nameResult.IsSuccess) return Result<Employee>.Failure(nameResult.Error);

        if(!addresses.Any()) return Result<Employee>.Failure("Een werknemer moet minstens één adres hebben.");

        var employee = new Employee
        {
            Name = nameResult.Value,
            DateOfBirth = dateOfBirth,
            Pin = EmployeeAuthentication.Create(pinHash),
            Addresses = [.. addresses]
        };

        return Result<Employee>.Success(employee);
    }


}
