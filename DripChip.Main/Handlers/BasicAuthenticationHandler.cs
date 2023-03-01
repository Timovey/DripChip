using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;
using DripChip.DataContracts.DataContracts.Auth;
using DripChip.Database.Implements;
using DripChip.Database.Interfaces;
using System.Text.RegularExpressions;
using DripChip.Main.Attributes;
using DripChip.Database.Models;

namespace DripChip.Main.Handlers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IAccountStorage _storage;

        private const string AUTH_HEADER = "Authorization";
        string EmailRegex = @"(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)";
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IAccountStorage storage)
            : base(options, logger, encoder, clock)
        {
            _storage = storage;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // для необязательной авторизации
            var endpoint = Context.GetEndpoint();
            bool isAnonimus = endpoint?.Metadata?.GetMetadata<NotStrictAttribute>() != null;

            if (isAnonimus && !Request.Headers.ContainsKey(AUTH_HEADER))
            {
                var claims = new[] {
                    new Claim(ClaimTypes.NameIdentifier, "0"),
                    new Claim(ClaimTypes.Email, "email@emaail.com"),
                };

                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return AuthenticateResult.Success(ticket);
            }
            if (!Request.Headers.ContainsKey(AUTH_HEADER))
            {
                return AuthenticateResult.Fail("Missing Authorization Header");
            }
            
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers[AUTH_HEADER]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
                var email = credentials.FirstOrDefault();
                var password = credentials.LastOrDefault();
                if (email == null || password == null)
                    return AuthenticateResult.Fail("Invalid Email or Password");

                if(!Regex.IsMatch(email, EmailRegex))
                {
                    return AuthenticateResult.Fail("Invalid Email");
                }
                var account = await _storage.AuthenticateAsync(new LoginContract
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
