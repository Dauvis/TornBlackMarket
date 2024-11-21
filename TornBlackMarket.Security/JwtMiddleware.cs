using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using TornBlackMarket.Common.Interfaces;

namespace TornBlackMarket.Security
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<JwtMiddleware> _logger;
        private readonly IProfileService _userService;
        private readonly double _expirationMinutes;

        public JwtMiddleware(RequestDelegate next, ILogger<JwtMiddleware> logger, IProfileService userService, IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _userService = userService;

            if (!double.TryParse(configuration["Jwt:ExpirationMinutes"], out _expirationMinutes))
            {
                _expirationMinutes = 60;
            }
        }

        public async Task Invoke(HttpContext context, ITornBlackMarketTokenUtil tokenUtil)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            try
            {
                string? profileId = null;
                DateTimeOffset expiration = DateTimeOffset.MinValue;

                (profileId, expiration) = tokenUtil.ValidateToken(token);

                if (profileId != null)
                {
                    var profile = await _userService.GetAsync(profileId);

                    // check for invalidated token
                    if (profile is not null && profile.TokenInvalidDateTime.AddMinutes(_expirationMinutes) > expiration)
                    {
                        profileId = null;
                        profile = null;
                    }

                    // Attach the user to the context.
                    context.Items["ProfileId"] = profile != null ? profileId : "";
                    context.Items["Profile"] = profile;
                }
                else
                {
                    context.Items["ProfileId"] = null;
                    context.Items["Profile"] = null;
                }
            }
            catch (SecurityTokenException ste)
            {
                _logger.LogError("Security token error: {Message}", ste.Message);
                context.Items["ProfileId"] = "";
            }

            await _next(context);
        }
    }
}
