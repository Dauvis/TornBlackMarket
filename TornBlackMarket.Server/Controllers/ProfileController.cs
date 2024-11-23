using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IProfileService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProfileController> _logger;
        private readonly IExchangeService _exchangeService;

        public ProfileController(IProfileService userService, IMapper mapper, ILogger<ProfileController> logger, IExchangeService exchangeService)
        {
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
            _exchangeService = exchangeService;
        }

        [HttpGet("{identifier?}")]
        public async Task<IActionResult> IndexAsync(string? identifier, [FromQuery] bool full = false)
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

                string profileId = (string)(string.IsNullOrEmpty(identifier) ? HttpContext.Items["ProfileId"] ?? "" : identifier);
                var profile = await _userService.GetAsync(profileId);

                if (profile == null)
                {
                    var errorResponse = new ErrorResponseDTO()
                    {
                        ErrorCode = ErrorCodesType.InvalidProfile,
                        ErrorMessage = "Profile for user not found."
                    };

                    return NotFound(errorResponse);
                }

                var tbmProfileDto = _mapper.Map<TbmProfileDTO>(profile);

                if (full)
                {
                    var exchange = await _exchangeService.GetAsync(profileId);
                    var tbmExchangeDto = _mapper.Map<TbmExchangeDTO>(exchange);
                    var responseDto = new TbmFullProfileDTO()
                    {
                        BasicProfile = tbmProfileDto,
                        Exchange = tbmExchangeDto
                    };

                    return Ok(responseDto);
                }

                return Ok(tbmProfileDto);
            }
            catch (Exception e)
            {
                _logger.LogError("{Exception} while attempting to fetch user profile: {Message}", e.GetType().Name, e.Message);
                throw;
            }
        }
    }
}
