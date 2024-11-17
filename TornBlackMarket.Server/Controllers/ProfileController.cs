using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TornBlackMarket.Common.DTO.Domain;
using TornBlackMarket.Common.DTO.External;
using TornBlackMarket.Common.Enums;
using TornBlackMarket.Common.Interfaces;

namespace TornBlackMarket.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : Controller
    {
        private readonly IUserProfileService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(IUserProfileService userService, IMapper mapper, ILogger<ProfileController> logger)
        {
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{identifier?}")]
        public async Task<IActionResult> IndexAsync(string? identifier)
        {
            try
            {
                if (HttpContext.Items["TornUser"] is null)
                {
                    var errorResponse = new ErrorResponseDTO()
                    {
                        ErrorCode = ErrorCodesType.NotAuthenticated,
                        ErrorMessage = "User is not authenticated"
                    };

                    return Unauthorized(errorResponse);
                }

                string userId = (string)(string.IsNullOrEmpty(identifier) ? HttpContext.Items["TornUserId"] ?? "" : identifier);
                var profile = await _userService.GetProfileAsync(userId);

                if (profile == null)
                {
                    var errorResponse = new ErrorResponseDTO()
                    {
                        ErrorCode = ErrorCodesType.InvalidProfile,
                        ErrorMessage = "Profile for user not found."
                    };

                    return NotFound(errorResponse);
                }

                var responseDTO = _mapper.Map<TbmProfileDTO>(profile);

                return Ok(responseDTO);
            }
            catch (Exception e)
            {
                _logger.LogError("{Exception} while attempting to fetch user profile: {Message}", e.GetType().Name, e.Message);
                throw;
            }
        }
    }
}
