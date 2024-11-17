using AutoMapper;
using Microsoft.Extensions.Configuration;
using TornBlackMarket.Common.Interfaces;
using TornBlackMarket.Common.DTO.Domain;
using Microsoft.Extensions.Logging;

namespace TornBlackMarket.Security
{
    public class TornApiKeyUtil: ITornApiKeyUtil
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly ITornApiService _tornApiService;
        private readonly ILogger<TornApiKeyUtil> _logger;
        private IUserProfileRepository? _profileRepository;

        public TornApiKeyUtil(IConfiguration configuration, IMapper mapper, IRepositoryFactory repositoryFactory, 
            ITornApiService tornApiService, ILogger<TornApiKeyUtil> logger)
        {
            _configuration = configuration;
            _mapper = mapper;
            _repositoryFactory = repositoryFactory;
            _tornApiService = tornApiService;
            _logger = logger;
        }

        public async Task<UserProfileDocumentDTO?> ProfileDocumentForApiKeyAsync(string? apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return null;
            }

            var userBasic = await _tornApiService.GetUserBasicAsync(apiKey);

            if (userBasic == null)
            {
                _logger.LogError("No response from Torn API");
                return null;
            }

            if (userBasic.Error is not null && userBasic.Error.Error.Code != 0)
            {
                throw new UnauthorizedAccessException($"Failure to authenticate with Torn API {userBasic.Error.Error.Code}: {userBasic.Error.Error.Message}");
            }

            if (userBasic.PlayerId != 0)
            {
                var repository = GetUserProfileRepository();
                var profile = await repository.GetAsync(userBasic.PlayerId.ToString());

                if (profile is null)
                {
                    var userInfoDto = new UserInfoDTO()
                    {
                        Id = userBasic.PlayerId.ToString(),
                        Name = userBasic.Name ?? ""
                    };

                    profile = await repository.CreateAsync(userInfoDto, apiKey);
                }

                // name or ApiKey changed, update the profile
                if (profile is not null && (profile.Name != userBasic.Name || profile.ApiKey != apiKey))
                {
                    profile.Name = userBasic.Name ?? profile.Name;
                    profile.ApiKey = apiKey;
                    await repository.UpdateAsync(profile);
                }

                return _mapper.Map<UserProfileDocumentDTO>(profile);
            }

            _logger.LogError("Torn API response lacks critical data (player_id)");
            return null;
        }

        protected IUserProfileRepository GetUserProfileRepository()
        {
            _profileRepository = _repositoryFactory.Create<IUserProfileRepository>();

            if (_profileRepository is null)
            {
                _logger.LogCritical("Failed to instantiate repository for interface {InterfaceName}", nameof(IUserProfileRepository));
                throw new InvalidOperationException($"Failed to instantiate repository: {nameof(IUserProfileRepository)}");
            }

            return _profileRepository;
        }
    }
}
