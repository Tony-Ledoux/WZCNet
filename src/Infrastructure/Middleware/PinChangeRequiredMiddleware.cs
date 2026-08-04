using System.Security.Claims;

namespace WZCNet.src.Infrastructure.Middleware;
public class PinChangeRequiredMiddleware(RequestDelegate next)
{
    private static readonly string[] _excludedPaths =
    [
        "/api/auth/identify",
        "/api/auth/change-pin",
        "/api/auth/login",
        "/api/auth/refresh"
    ];

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var requiresPinChange = context.User.FindFirstValue("RequiresPinChange");
            var path = context.Request.Path.Value?.ToLower();

            if (requiresPinChange?.Equals("true", StringComparison.OrdinalIgnoreCase) == true 
    && !_excludedPaths.Contains(path))
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsJsonAsync(new 
                { 
                    error = "Pin change required before accessing this resource." 
                });
                return;
            }
        }

        await next(context);
    }
}