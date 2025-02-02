using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace Demo.Authentication;

// A custom authentication handler that derives from AuthenticationHandler<TOptions>.
// The generic type TOptions is the type of authentication options that the handler works with.
public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    // Constructor for BasicAuthenticationHandler.
    // parameters:
    // - IOptionsMonitor<AuthenticationSchemeOptions> options: Monitors changes to authentication scheme options
    // - ILoggerFactory logger: Factory to create logger instances
    // - UrlEncoder encoder: Encodes URLs to ensure they are safe
    // - IUserService userService: The service used to validate user credentials.
    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return AuthenticateResult.Fail("Missing Authorization Header");
        }

        var authorizationHeader = Request.Headers["Authorization"].ToString();

        if (!AuthenticationHeaderValue.TryParse(authorizationHeader, out var headerValue))
        {
            return AuthenticateResult.Fail("Invalid Authorization Header");
        }

        if (!"Basic".Equals(headerValue.Scheme, StringComparison.OrdinalIgnoreCase))
        {
            return AuthenticateResult.Fail("Invalid Authorization Scheme");
        }

        // This yields a "username:password" string which is then split by the colon.
        var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(headerValue.Parameter)).Split(':', 2);

        // Check if splitting the credentials results in exactly two components (username and password).
        if (credentials.Length != 2)
        {
            return AuthenticateResult.Fail("Invalid Basic Authentication Credentials");
        }

        var userName = credentials[0];
        var password = credentials[1];

        try
        {
            if (!ValidateAdminCredentials(userName, password))
            {
                return AuthenticateResult.Fail("Invalid Username or Password");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, "admin"),
            };

            var claimsIdentity = new ClaimsIdentity(claims, "Basic");

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var authenticationTicket = new AuthenticationTicket(claimsPrincipal, "Basic");

            // Indicate that authentication was successful and return the ticket
            return AuthenticateResult.Success(authenticationTicket);
        }
        catch
        {
            return AuthenticateResult.Fail("Error occurred during authentication");
        }
    }

    private bool ValidateAdminCredentials(string userName, string password)
    {
        string? envUserName = Environment.GetEnvironmentVariable("ADMIN_USERNAME");
        string? envPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");

        if (envUserName == null || envPassword == null || userName == null  || password == null)
        {
            return false;
        }
        
        if (envUserName == "" || envPassword == "" || userName == "" || password == "")
        {
            return false;
        }

        return userName == envUserName && password == envPassword;
    }
}