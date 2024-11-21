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
        private readonly IProfileService _profileService;
        private readonly ILogger<LoginController> _logger;

        public LoginController(IProfileService profileService, ILogger<LoginController> logger)
        {
            _profileService = profileService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> IndexAsync([FromBody] AuthenticationRequestDTO request)
        {
            try
            {
                var response = await _profileService.AuthenticateAsync(request);

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

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> InvalidateTokensAsync()
        {
            try
            {
                if (HttpContext.Items["Profile"] is null)
                {
                    var errorResponse = new ErrorResponseDTO()
                    {
                        ErrorCode = ErrorCodesType.NotAuthenticated,
                        ErrorMessage = "User is not authenticated"
                    };

                    return Unauthorized(errorResponse);
                }

                string profileId = (string?)HttpContext.Items["ProfileId"] ?? "";
                
                if (await _profileService.InvalidateTokensAsync(profileId))
                {
                    return new NoContentResult();
                }

                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError("{Exception} while attempting to log out user: {Message}", e.GetType().Name, e.Message);
                throw;
            }
        }
    }
}
