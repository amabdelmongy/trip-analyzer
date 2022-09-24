namespace TripAnalyzer.Api.Middleware;

public interface IAuthenticationService
{
    bool IsAuthenticated(string requestApiKey);
}

public class AuthenticationService : IAuthenticationService
{
    public bool IsAuthenticated(string requestApiKey)
    {
            if (string.IsNullOrWhiteSpace(requestApiKey) || !requestApiKey.StartsWith("Basic"))
                return false;

            try
            {
                var usernamePassword = Encoding.GetEncoding("iso-8859-1")
                    .GetString(Convert.FromBase64String(requestApiKey.Substring("Basic ".Length).Trim()));

                var separatorIndex = usernamePassword.IndexOf(':');
                var userName = usernamePassword.Substring(0, separatorIndex);
                var password = usernamePassword.Substring(separatorIndex + 1);

                return userName == "demo" && password == "demo";
            }
            catch
            {
                // do nothing if invalid auth header
                // user is not attached to context so request won't have access to secure routes
            }

            return false;
    }
}