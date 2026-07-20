
using WZCNet.src.Domain.Common;

namespace WZCNet.src.Domain.ValueObjects;

public sealed class Name
{
    public string First {get;}
    public string Last {get;}

    public static Result<Name> Create(string first, string last)
    {
        ArgumentException.ThrowIfNullOrEmpty(first, nameof(first));
        ArgumentException.ThrowIfNullOrEmpty(last, nameof(last));

        return Result<Name>.Success(new Name(first.Trim(),last.Trim()));
    }

    private Name(string first, string last)
    {
        First = first;
        Last = last;
    }
}
