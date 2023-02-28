using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;
using DripChip.Core.BusinessLogic;
using DripChip.DataContracts.DataContracts.Auth;

namespace DripChip.Main.Handlers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly AccountLogic _logic;

        private const string AUTH_HEADER = "Authorization";
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            AccountLogic logic)
            : base(options, logger, encoder, clock)
        {
            _logic = logic;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // skip authentication if endpoint has [AllowAnonymous] attribute
            var endpoint = Context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
                return AuthenticateResult.NoResult();

            if (!Request.Headers.ContainsKey(AUTH_HEADER))
                return AuthenticateResult.Fail("Missing Authorization Header");
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers[AUTH_HEADER]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
                var email = credentials.FirstOrDefault();
                var password = credentials.LastOrDefault();
                if (email == null || password == null)
                    return AuthenticateResult.Fail("Invalid Email or Password");

                var account = await _logic.Authenticate(new LoginContract
                {
                    Email= email, 
                    Password = password
                });

                if (account == null)
                    return AuthenticateResult.Fail("Invalid Email or Password");

                var claims = new[] {
                    new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                    new Claim(ClaimTypes.Email, account.Email),
                };

                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }
        }
    }
}
