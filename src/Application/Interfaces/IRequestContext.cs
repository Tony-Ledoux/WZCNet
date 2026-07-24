using WZCNet.src.Domain.Entities;

namespace WZCNet.src.Application.Interfaces;

public interface IRequestContext
{
    string? IpAddress { get; }
    string? DeviceInfo { get; }
}
