using System.Security.Claims;

namespace TornBlackMarket.Common.Interfaces
{
    public interface ITornBlackMarketTokenUtil
    {
        string GenerateToken(ClaimsPrincipal claimsPrincipal);
        string? ValidateToken(string? token);
    }
}
