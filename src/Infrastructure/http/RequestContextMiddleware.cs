using System;
using WZCNet.src.Application.Interfaces;

namespace WZCNet.src.Infrastructure.http;

public sealed class RequestContextMiddleware(RequestDelegate next)
{
   public async Task InvokeAsync(HttpContext context, RequestContext requestContext)
    {
        requestContext.IpAddress = context.Connection.RemoteIpAddress?.ToString();
        requestContext.DeviceInfo = context.Request.Headers.UserAgent.ToString();
        await next(context);
    }
}
