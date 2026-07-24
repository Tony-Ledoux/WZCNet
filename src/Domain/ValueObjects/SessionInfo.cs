
namespace WZCNet.src.Domain.ValueObjects;

public  class SessionInfo
{
    public string? DeviceInfo {get;}
    public string? IpAddress {get;}
    private SessionInfo(){}
    private SessionInfo(string? device, string? ip)
    {
        DeviceInfo = device;
        IpAddress = ip;
    }

    public static SessionInfo Create(string? device, string? ip) => new(device,ip);

}
