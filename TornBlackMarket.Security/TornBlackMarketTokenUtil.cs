using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using TornBlackMarket.Common.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TornBlackMarket.Security
{
    public class TornBlackMarketTokenUtil: ITornBlackMarketTokenUtil
    {
        private readonly string _secretKey;
        private readonly string _validIssuer;
        private readonly string _validAudience;
        private readonly ILogger<TornBlackMarketTokenUtil> _logger;

        public TornBlackMarketTokenUtil(IConfiguration configuration, ILogger<TornBlackMarketTokenUtil> logger)
        {
            _logger = logger;
            _secretKey = configuration["TORN_SECRET_KEY"] ?? throw new ArgumentException("JWT secret key has not been set", nameof(configuration));
            _validIssuer = configuration["Jwt:ValidIssuer"] ?? throw new ArgumentException("JWT valid issuer has not been set", nameof(configuration));
            _validAudience = configuration["Jwt:ValidAudience"] ?? throw new ArgumentException("JWT valid audience has not been set", nameof(configuration));
        }

        public string? ValidateToken(string? token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(GetKeyBytes()),
                ValidateIssuer = true,
                ValidIssuer = _validIssuer,
                ValidateAudience = true,
                ValidAudience = _validAudience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                var profileId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                return profileId;
            }
            catch (Exception ex)
            {
                // Handle validation failure
                _logger.LogError("Token validation failed: {Message}", ex.Message);
                return null;
            }
        }

        public string GenerateToken(ClaimsPrincipal claimsPrincipal)
        {
            var securityKey = new SymmetricSecurityKey(GetKeyBytes());
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                issuer: _validIssuer,
                audience: _validAudience,
                claims: claimsPrincipal.Claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        private byte[] GetKeyBytes()
        {
            return Encoding.UTF8.GetBytes(_secretKey);
        }
    }
}
