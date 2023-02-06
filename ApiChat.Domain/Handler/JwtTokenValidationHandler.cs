using ApiChat.Domain.UnitOfWork;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ApiChat.Domain.Handler
{
    public class JwtTokenValidationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {


        public JwtTokenValidationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IUnitOfWork unitOfWork)
            : base(options, logger, encoder, clock)
        {

        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string authorization = Request.Headers[HeaderNames.Authorization];

            if (string.IsNullOrEmpty(authorization))
            {
                return Task.FromResult(AuthenticateResult.Fail("Authorization header not found"));
            }

            if (!authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid authorization header"));
            }

            var token = authorization.Substring("Bearer ".Length).Trim();

            var handler = new JwtSecurityTokenHandler();

            if (!handler.CanReadToken(token))
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid JWT token"));
            }

            var jwtToken = handler.ReadJwtToken(token);

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(jwtToken.Claims, Scheme.Name));

            var authenticationTicket = new AuthenticationTicket(claimsPrincipal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(authenticationTicket));


        }
    }
}
