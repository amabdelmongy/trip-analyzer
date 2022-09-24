namespace TripAnalyzer.Api.Middleware;

public class ApiKeyAuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    public ApiKeyAuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext, IAuthenticationService authenticationService)
    {
        if (httpContext is null)
            throw new ArgumentNullException(nameof(httpContext));

        var path = httpContext.Request.Path.Value;
        if (!string.IsNullOrEmpty(path) &&
            (path.Contains("/health", StringComparison.InvariantCulture) ||
            path.Contains("/swagger", StringComparison.InvariantCulture))
        )
        {
            await _next(httpContext);
            return;
        }

        if (!authenticationService.IsAuthenticated(httpContext.Request.Headers["Authorization"]))
        {
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        await _next(httpContext);
    }
}
