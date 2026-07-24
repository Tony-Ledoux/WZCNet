using System;
using WZCNet.src.Application.Interfaces;

namespace WZCNet.src.Infrastructure.http;

public class RequestContext: IRequestContext
{
    public string? IpAddress { get; internal set; }
    public string? DeviceInfo { get; internal set; }
}
