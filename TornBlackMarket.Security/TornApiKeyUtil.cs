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
        private readonly IEncryptionUtil _encryptionUtil;
        private IProfileRepository? _profileRepository;

        public TornApiKeyUtil(IConfiguration configuration, IMapper mapper, IRepositoryFactory repositoryFactory, 
            ITornApiService tornApiService, ILogger<TornApiKeyUtil> logger, IEncryptionUtil encryptionUtil)
        {
            _configuration = configuration;
            _mapper = mapper;
            _repositoryFactory = repositoryFactory;
            _tornApiService = tornApiService;
            _logger = logger;
            _encryptionUtil = encryptionUtil;
        }

        public async Task<(ProfileDocumentDTO?, bool)> ProfileDocumentForApiKeyAsync(string? apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return (null, false);
            }

            var userBasic = await _tornApiService.GetUserBasicAsync(apiKey);

            if (userBasic == null)
            {
                _logger.LogError("No response from Torn API");
                return (null, false);
            }

            if (userBasic.Error is not null && userBasic.Error.Error.Code != 0)
            {
                throw new UnauthorizedAccessException($"Failure to authenticate with Torn API {userBasic.Error.Error.Code}: {userBasic.Error.Error.Message}");
            }

            if (userBasic.PlayerId != 0)
            {
                var repository = GetUserProfileRepository();
                var profile = await repository.GetAsync(userBasic.PlayerId.ToString());
                bool isNewPlayer = false;

                if (profile is null)
                {
                    var newProfileDto = new ProfileDocumentDTO()
                    {
                        Id = userBasic.PlayerId.ToString(),
                        Name = userBasic.Name ?? ""
                    };

                    profile = await repository.CreateAsync(newProfileDto, apiKey);
                    isNewPlayer = true;
                }

                // name or ApiKey changed, update the profile
                if (profile is not null)
                {
                    string encryptedCheck = _encryptionUtil.Encrypt(apiKey, profile.ApiKeyVI);

                    if (profile.Name != userBasic.Name || profile.ApiKey != encryptedCheck)
                    {
                        profile.Name = userBasic.Name ?? profile.Name;
                        await repository.UpdateAsync(profile, apiKey);
                    }
                }

                return (_mapper.Map<ProfileDocumentDTO>(profile), isNewPlayer);
            }

            _logger.LogError("Torn API response lacks critical data (player_id)");
            return (null, false);
        }

        protected IProfileRepository GetUserProfileRepository()
        {
            _profileRepository = _repositoryFactory.Create<IProfileRepository>();

            if (_profileRepository is null)
            {
                _logger.LogCritical("Failed to instantiate repository for interface {InterfaceName}", nameof(IProfileRepository));
                throw new InvalidOperationException($"Failed to instantiate repository: {nameof(IProfileRepository)}");
            }

            return _profileRepository;
        }
    }
}
