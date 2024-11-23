using Microsoft.AspNetCore.Http;
using TornBlackMarket.Common.DTO.External;
using TornBlackMarket.Common.Enums;

namespace TornBlackMarket.Security
{
    public static class AuthenticationUtil
    {
        public static bool CheckAuthentication(HttpContext httpContext, out ErrorResponseDTO? response)
        {
            if (httpContext.Items["Profile"] is null)
            {
                response = new ErrorResponseDTO()
                {
                    ErrorCode = ErrorCodesType.NotAuthenticated,
                    ErrorMessage = "User is not authenticated"
                };

                return false;
            }

            response = null;

            return true;
        }
    }
}
