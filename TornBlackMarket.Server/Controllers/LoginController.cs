using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TornBlackMarket.Common.DTO.External;
using TornBlackMarket.Common.Enums;
using TornBlackMarket.Common.Interfaces;

namespace TornBlackMarket.Server.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly IProfileService _userService;
        private readonly ILogger<LoginController> _logger;

        public LoginController(IProfileService userService, ILogger<LoginController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> IndexAsync([FromBody] AuthenticationRequestDTO request)
        {
            try
            {
                var response = await _userService.AuthenticateAsync(request);

                if (response.ErrorCode == ErrorCodesType.None)
                {
                    return Ok(response);
                }
                else
                {
                    ErrorResponseDTO errorResponse = new()
                    {
                        ErrorCode = response.ErrorCode,
                        ErrorMessage = response.ErrorMessage
                    };

                    if (errorResponse.ErrorCode == ErrorCodesType.AuthenticationFailed)
                    {
                        return Unauthorized(errorResponse);
                    }

                    return new ObjectResult(errorResponse)
                    {
                        StatusCode = 403
                    };
                }
            }
            catch (Exception e)
            {
                _logger.LogError("{Exception} while attempting to authenticate user: {Message}", e.GetType().Name, e.Message);
                throw;
            }
        }
    }
}
