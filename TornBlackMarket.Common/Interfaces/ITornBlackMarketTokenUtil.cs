using System.Security.Claims;

namespace TornBlackMarket.Common.Interfaces
{
    public interface ITornBlackMarketTokenUtil
    {
        string GenerateToken(ClaimsPrincipal claimsPrincipal);
        (string?, DateTimeOffset) ValidateToken(string? token);
    }
}
