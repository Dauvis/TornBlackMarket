using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TornBlackMarket.Common.DTO.Domain;
using TornBlackMarket.Common.DTO.External;
using TornBlackMarket.Common.Enums;
using TornBlackMarket.Common.Interfaces;
using TornBlackMarket.Security;

namespace TornBlackMarket.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProfileController> _logger;
        private readonly IExchangeService _exchangeService;

        public ProfileController(IProfileService profileService, IMapper mapper, ILogger<ProfileController> logger, IExchangeService exchangeService)
        {
            _profileService = profileService;
            _mapper = mapper;
            _logger = logger;
            _exchangeService = exchangeService;
        }

        [HttpGet("{identifier?}")]
        public async Task<IActionResult> IndexAsync([FromRoute] string? identifier, [FromQuery] bool full = false)
        {
            try
            {
                if (!AuthenticationUtil.CheckAuthentication(HttpContext, out var errorResponse))
                {
                    return Unauthorized(errorResponse);
                }

                string profileId = (string)(string.IsNullOrEmpty(identifier) ? HttpContext.Items["ProfileId"] ?? "" : identifier);
                var profile = await _profileService.GetAsync(profileId);

                if (profile == null)
                {
                    var profileErrorResponse = new ErrorResponseDTO()
                    {
                        ErrorCode = ErrorCodesType.InvalidProfile,
                        ErrorMessage = "Profile for user not found."
                    };

                    return NotFound(profileErrorResponse);
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
                _logger.LogError("{Exception} while attempting to fetch profile {ProfileId}: {Message}", e.GetType().Name, 
                    HttpContext.Items["ProfileId"], e.Message);
                throw;
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] TbmProfileDTO profile)
        {
            try
            {
                if (!AuthenticationUtil.CheckAuthentication(HttpContext, out var errorResponse))
                {
                    return Unauthorized(errorResponse);
                }

                var profileDto = _mapper.Map<ProfileDocumentDTO>(profile);
                string profileId = (string?)HttpContext.Items["ProfileId"] ?? "";
                bool result = await _profileService.UpdateAsync(profileId, profileDto);

                if (result)
                {
                    return NoContent();
                }

                var updateErrorResponse = new ErrorResponseDTO()
                {
                    ErrorCode = ErrorCodesType.ProfileUpdateFailure,
                    ErrorMessage = (profileId == profileDto.Id) ? $"Failed to update profile {profileDto.Id}" :
                        $"Illegal attempt to update profile {profileDto.Id}"
                };

                return BadRequest(updateErrorResponse);
            }
            catch (Exception e)
            {
                _logger.LogError("{Exception} while attempting to update profile {ProfileId}: {Message}", e.GetType().Name,
                    HttpContext.Items["ProfileId"], e.Message);
                throw;
            }
        }
    }
}
