using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using TornBlackMarket.Common.Interfaces;

namespace TornBlackMarket.Security
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<JwtMiddleware> _logger;
        private readonly IUserProfileService _userService;

        public JwtMiddleware(RequestDelegate next, ILogger<JwtMiddleware> logger, IUserProfileService userService)
        {
            _next = next;
            _logger = logger;
            _userService = userService;
        }

        public async Task Invoke(HttpContext context, ITornBlackMarketTokenUtil tokenUtil)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            try
            {
                var userId = tokenUtil.ValidateToken(token);
                context.Items["TornUserId"] = userId;

                if (userId != null)
                {
                    // Attach the user to the context.
                    context.Items["TornUser"] = await _userService.GetAsync(userId);
                }
            }
            catch (SecurityTokenException ste)
            {
                _logger.LogError("Security token error: {Message}", ste.Message);
                context.Items["TornUserId"] = "";
            }

            await _next(context);
        }
    }
}
